using System;
using System.Text;
using System.Security;
using System.Diagnostics;

namespace Sha1Task
{
	/// <summary>
	/// Description of Sha1Alg.
	/// </summary>
	public class Sha1Alg
	{
		#region Поля
		private int _HashBitCount = 62; //Число значимых бит в хеше
		
		private byte[] _Buffer; //временный буфер для вычисления хешей
        private long   _Count; //количество байт в хеш-сообщении
        private uint[] _StateSHA1; //тут состояние хеша на каждом этапе и итоговое
        private uint[] _ExpandedBuffer; //на этапах вычисления используется расширенный буфер(16 32-битовых слов превращаем в 80 32-битовых слов)
		#endregion
		
		#region Свойства
			#region public int HashBitCount. Число значимых бит в хеше
			/// <summary>
			/// Число значимых бит в хеше
			/// </summary>
			public int HashBitCount
			{
				set{
					_HashBitCount = value;
				}
				get{
					return _HashBitCount;
				}
			}
			#endregion
		#endregion
		
		#region Конструктор
		public Sha1Alg()
		{
			_StateSHA1 = new uint[5]; //длина хеша 160 бит
            _Buffer = new byte[64]; //сообщение разбивается на блоки по 512 бит
            _ExpandedBuffer = new uint[80]; //расширенный буфер 80 32-битных слов
		}
		#endregion
		
		#region Методы
			#region public GetHash. Возвращает sha1-хеш для переданной строки
			public Sha1Hash GetHash(string text)
			{
				return GetHash(text, Encoding.GetEncoding("utf-8"/*1251*/));
			}
			public Sha1Hash GetHash(string text, Encoding enc)
			{
				Sha1Hash result = new Sha1Hash();
				
				byte[] buffer = enc.GetBytes(text); //преобразуем переданный текст в набор байт с учетом кодировки
				
				InitializeState(); //задаем начальные значения
				HashData(buffer, 0, buffer.Length); //вычисляем все блоки кроме последнего
				
				result.Value = EndHash(); //вычисляем последний блок и фиксируем результат
				result.ByteCount = result.Value.Length;
				
				return result;
			}
			#endregion
			#region public GetTruncateHash. Преобразует sha1-хеш в усеченный хеш с указанным количеством бит
			/// <summary>
			/// Преобразует sha1-хеш в усеченный хеш с указанным количеством бит
			/// </summary>
			public Sha1Hash GetTruncateHash(byte[] sha1Value)
			{
				return GetTruncateHash(sha1Value, HashBitCount);
			}
			public Sha1Hash GetTruncateHash(byte[] sha1Value, int hashBitCount)
			{
				Sha1Hash result = null;
				
				if (sha1Value != null && sha1Value.Length > 0 && hashBitCount > 0)
				{
					result = new Sha1Hash();	
					
					if (hashBitCount < sha1Value.Length * 8)
					{
						int byteCount = hashBitCount/8; //получаем количество целых байт
						int lastBitsCount = hashBitCount%8; //если есть остаток, недотягивающий до байта
	
						int needLength = byteCount + (lastBitsCount == 0 ? 0: 1); //необходимое количество байт
						
						result.Value = new byte[needLength];
						
						//сначала запишем целые байты			
						for (int i=0; i < byteCount; i++)
						{
							result.Value[i] = sha1Value[i];
						}
						
						if (lastBitsCount > 0) //если есть остаток незаписанных бит
						{
							byte mask = (byte)(2<<(lastBitsCount - 1) - 1); //получаем маску значимых бит
							
							//и записываем значимые биты в последний байт
							result.Value[byteCount] = (byte) (sha1Value[byteCount] & mask);
							result.ByteCount = needLength;	
						}
					}
					else //если обрезать нечего
					{
						result.Value = sha1Value;
						result.ByteCount = sha1Value.Length;
					}
				}
				
				return result;
			}
			#endregion
			#region public Compare. Сравнивает переданные хеши между собой с учетом усечения
			public bool Compare(Sha1Hash hashX, Sha1Hash hashY)
			{
				bool result = false;
				
				if (hashX != null && hashY != null)
					result = Compare(hashX.Value, hashY.Value);
					
				return result;
			}
			public bool Compare(byte[] hashX, byte[] hashY)
			{
				return Compare(hashX, hashY, HashBitCount);
			}
			public bool Compare(byte[] hashX, byte[] hashY, int hashBitsNum)
			{
				if (hashX != null && hashY != null && hashBitsNum > 0)
				{
					int byteCount = hashBitsNum/8; //получаем количество целых байт
					int lastBitsCount = hashBitsNum%8; //если есть остаток, недотягивающий до байта

					int needLength = byteCount + (lastBitsCount == 0 ? 0: 1); //необходимое количество элементов в переданных массивах
					
					if (hashX.Length >= needLength && hashY.Length >= needLength)
					{
						//сначала проверим побайтно				
						for (int i=0; i < byteCount; i++)
						{
							if (hashX[i] != hashY[i])
								return false;
						}
						
						if (lastBitsCount > 0) //если есть остаток непроверенных бит
						{
							byte mask = (byte)(2<<(lastBitsCount - 1) - 1); //получаем маску значимых бит
							
							//и проверяем значимые биты на совпадение
							if ((hashX[byteCount] & mask) != (hashY[byteCount] & mask))
								return false;
						}
						
						return true; //только если дошли до сюда, то все ок
					}
				}
					
				return false;
			}
			#endregion
			
		//"Рабочие методы" (использующиеся для вычисления хеша)	
			#region private InitializeState. Проводим первичную инициализацию 
			private void InitializeState() 
			{
	            _Count = 0;
	  
	            _StateSHA1[0] =  0x67452301;
	            _StateSHA1[1] =  0xefcdab89;
	            _StateSHA1[2] =  0x98badcfe;
	            _StateSHA1[3] =  0x10325476;
	            _StateSHA1[4] =  0xc3d2e1f0;
        	}	
			#endregion
			#region private HashData. Вычисляем хеш основной части сообщения
			private unsafe void HashData(byte[] partIn, int ibStart, int cbSize)
	        {
	            int bufferLen;
	            int partInLen = cbSize;
	            int partInBase = ibStart;
	  
	            //Вычисляем длину буфера
	            bufferLen = (int) (_Count & 0x3f);
	  
	            //Обновляем количество байт
	            _Count += partInLen;
	 
	            //фиксируем необходимые указатели, чтобы они не были перемещены
	            fixed (uint* stateSHA1 = _StateSHA1) {
	                fixed (byte* buffer = _Buffer) {
	                    fixed (uint* expandedBuffer = _ExpandedBuffer) {
	                        if ((bufferLen > 0) && (bufferLen + partInLen >= 64)) {
	                            Buffer.BlockCopy(partIn, partInBase, _Buffer, bufferLen, 64 - bufferLen);
	                            partInBase += (64 - bufferLen);
	                            partInLen -= (64 - bufferLen);
	                            SHATransform(expandedBuffer, stateSHA1, buffer);
	                            bufferLen = 0;
	                        }
	 
	                        //Копируем во временный буфер и рассчитываем хеш всех блоков кроме последнего
	                        while (partInLen >= 64) 
	                        {
	                        	//готовим очередной блок 512 бит
	                            Buffer.BlockCopy(partIn, partInBase, _Buffer, 0, 64);
	                            partInBase += 64;
	                            partInLen -= 64;
	                            
	                            //производим вычисление его хеша
	                            SHATransform(expandedBuffer, stateSHA1, buffer);
	                        }
	  
	                        //остался хвост? фиксируем:
	                        if (partInLen > 0) {
	                            Buffer.BlockCopy(partIn, partInBase, _Buffer, bufferLen, partInLen);
	                        }
	                    }
	                }
	            }
	        }
	 		#endregion
	        #region private EndHash. Обсчитываем последний блок сообщения и возвращаем результирующий хеш
	        private byte[] EndHash()
	        {
	            byte[]          pad;
	            int             padLen;
	            long            bitCount;
	            byte[]          hash = new byte[20];
	 
	            //Формируем последний блок сообщения. Суть формирования из вики:
	            /*
	            * Последний блок дополняется до длины, кратной 512 бит. 
				* Сначала добавляется 1, а потом нули, чтобы длина блока стала равной 
				* (512 - 64 = 448) бит. В оставшиеся 64 бита записывается длина исходного 
				* сообщения в битах. Если последний блок имеет длину более 448, но менее 512 бит, 
				* то дополнение выполняется следующим образом: сначала добавляется 1, затем нули 
				* вплоть до конца 512-битного блока; после этого создается ещё один 512-битный блок, 
				* который заполняется вплоть до 448 бит нулями, после чего в оставшиеся 64 бита 
				* записывается длина исходного сообщения в битах. Дополнение последнего блока 
				* осуществляется всегда, даже если сообщение уже имеет нужную длину.
	            */
	 
	            padLen = 64 - (int)(_Count & 0x3f);
	            if (padLen <= 8)
	                padLen += 64;
	 
	            pad = new byte[padLen];
	            pad[0] = 0x80;
	  
	            //Приводим к количеству бит:
	            bitCount = _Count * 8;
	  
	            pad[padLen-8] = (byte) ((bitCount >> 56) & 0xff);
	            pad[padLen-7] = (byte) ((bitCount >> 48) & 0xff);
	            pad[padLen-6] = (byte) ((bitCount >> 40) & 0xff);
	            pad[padLen-5] = (byte) ((bitCount >> 32) & 0xff);
	            pad[padLen-4] = (byte) ((bitCount >> 24) & 0xff);
	            pad[padLen-3] = (byte) ((bitCount >> 16) & 0xff);
	            pad[padLen-2] = (byte) ((bitCount >> 8) & 0xff);
	            pad[padLen-1] = (byte) ((bitCount >> 0) & 0xff);
	  
	            //Вычисляем хеш последнего блока
	            HashData(pad, 0, pad.Length);
	 
	            //преобразуем 5х32 слов к результирующему набору байт
	            DWORDToBigEndian (hash, _StateSHA1, 5);
	  
	            return hash;
	        }
	 		#endregion
	 		#region private SHATransform. Главный цикл хеширования
	        private static unsafe void SHATransform (uint* expandedBuffer, uint* state, byte* block)
	        {
	            uint a = state[0];
	            uint b = state[1];
	            uint c = state[2];
	            uint d = state[3];
	            uint e = state[4];
	 
	            int i;
	 
	            DWORDFromBigEndian(expandedBuffer, 16, block); //преобразуем к нужному порядку следования
	            SHAExpand(expandedBuffer); //блок сообщения преобразуется из 16 32-битовых слов в 80 32-битовых слов
	  
	            //Этап 1
	            for (i=0; i<20; i+= 5) {
	                { (e) +=  (((((a)) << (5)) | (((a)) >> (32-(5)))) + ( (d) ^ ( (b) & ( (c) ^ (d) ) ) ) + (expandedBuffer[i]) + 0x5a827999); (b) =  ((((b)) << (30)) | (((b)) >> (32-(30)))); }
	                { (d) +=  (((((e)) << (5)) | (((e)) >> (32-(5)))) + ( (c) ^ ( (a) & ( (b) ^ (c) ) ) ) + (expandedBuffer[i+1]) + 0x5a827999); (a) =  ((((a)) << (30)) | (((a)) >> (32-(30)))); }
	                { (c) +=  (((((d)) << (5)) | (((d)) >> (32-(5)))) + ( (b) ^ ( (e) & ( (a) ^ (b) ) ) ) + (expandedBuffer[i+2]) + 0x5a827999); (e) =  ((((e)) << (30)) | (((e)) >> (32-(30)))); };;
	                { (b) +=  (((((c)) << (5)) | (((c)) >> (32-(5)))) + ( (a) ^ ( (d) & ( (e) ^ (a) ) ) ) + (expandedBuffer[i+3]) + 0x5a827999); (d) =  ((((d)) << (30)) | (((d)) >> (32-(30)))); };;
	                { (a) +=  (((((b)) << (5)) | (((b)) >> (32-(5)))) + ( (e) ^ ( (c) & ( (d) ^ (e) ) ) ) + (expandedBuffer[i+4]) + 0x5a827999); (c) =  ((((c)) << (30)) | (((c)) >> (32-(30)))); };;
	            }
	  
	            //Этап 2
	            for (; i<40; i+= 5) {
	                { (e) +=  (((((a)) << (5)) | (((a)) >> (32-(5)))) + ((b) ^ (c) ^ (d)) + (expandedBuffer[i]) + 0x6ed9eba1); (b) =  ((((b)) << (30)) | (((b)) >> (32-(30)))); };;
	                { (d) +=  (((((e)) << (5)) | (((e)) >> (32-(5)))) + ((a) ^ (b) ^ (c)) + (expandedBuffer[i+1]) + 0x6ed9eba1); (a) =  ((((a)) << (30)) | (((a)) >> (32-(30)))); };;
	                { (c) +=  (((((d)) << (5)) | (((d)) >> (32-(5)))) + ((e) ^ (a) ^ (b)) + (expandedBuffer[i+2]) + 0x6ed9eba1); (e) =  ((((e)) << (30)) | (((e)) >> (32-(30)))); };;
	                { (b) +=  (((((c)) << (5)) | (((c)) >> (32-(5)))) + ((d) ^ (e) ^ (a)) + (expandedBuffer[i+3]) + 0x6ed9eba1); (d) =  ((((d)) << (30)) | (((d)) >> (32-(30)))); };;
	                { (a) +=  (((((b)) << (5)) | (((b)) >> (32-(5)))) + ((c) ^ (d) ^ (e)) + (expandedBuffer[i+4]) + 0x6ed9eba1); (c) =  ((((c)) << (30)) | (((c)) >> (32-(30)))); };;
	            }
	  
	            //Этап 3
	            for (; i<60; i+=5) {
	                { (e) +=  (((((a)) << (5)) | (((a)) >> (32-(5)))) + ( ( (b) & (c) ) | ( (d) & ( (b) | (c) ) ) ) + (expandedBuffer[i]) + 0x8f1bbcdc); (b) =  ((((b)) << (30)) | (((b)) >> (32-(30)))); };;
	                { (d) +=  (((((e)) << (5)) | (((e)) >> (32-(5)))) + ( ( (a) & (b) ) | ( (c) & ( (a) | (b) ) ) ) + (expandedBuffer[i+1]) + 0x8f1bbcdc); (a) =  ((((a)) << (30)) | (((a)) >> (32-(30)))); };;
	                { (c) +=  (((((d)) << (5)) | (((d)) >> (32-(5)))) + ( ( (e) & (a) ) | ( (b) & ( (e) | (a) ) ) ) + (expandedBuffer[i+2]) + 0x8f1bbcdc); (e) =  ((((e)) << (30)) | (((e)) >> (32-(30)))); };;
	                { (b) +=  (((((c)) << (5)) | (((c)) >> (32-(5)))) + ( ( (d) & (e) ) | ( (a) & ( (d) | (e) ) ) ) + (expandedBuffer[i+3]) + 0x8f1bbcdc); (d) =  ((((d)) << (30)) | (((d)) >> (32-(30)))); };;
	                { (a) +=  (((((b)) << (5)) | (((b)) >> (32-(5)))) + ( ( (c) & (d) ) | ( (e) & ( (c) | (d) ) ) ) + (expandedBuffer[i+4]) + 0x8f1bbcdc); (c) =  ((((c)) << (30)) | (((c)) >> (32-(30)))); };;
	            }
	  
	            //Этап 4
	            for (; i<80; i+=5) {
	                { (e) +=  (((((a)) << (5)) | (((a)) >> (32-(5)))) + ((b) ^ (c) ^ (d)) + (expandedBuffer[i]) + 0xca62c1d6); (b) =  ((((b)) << (30)) | (((b)) >> (32-(30)))); };;
	                { (d) +=  (((((e)) << (5)) | (((e)) >> (32-(5)))) + ((a) ^ (b) ^ (c)) + (expandedBuffer[i+1]) + 0xca62c1d6); (a) =  ((((a)) << (30)) | (((a)) >> (32-(30)))); };;
	                { (c) +=  (((((d)) << (5)) | (((d)) >> (32-(5)))) + ((e) ^ (a) ^ (b)) + (expandedBuffer[i+2]) + 0xca62c1d6); (e) =  ((((e)) << (30)) | (((e)) >> (32-(30)))); };;
	                { (b) +=  (((((c)) << (5)) | (((c)) >> (32-(5)))) + ((d) ^ (e) ^ (a)) + (expandedBuffer[i+3]) + 0xca62c1d6); (d) =  ((((d)) << (30)) | (((d)) >> (32-(30)))); };;
	                { (a) +=  (((((b)) << (5)) | (((b)) >> (32-(5)))) + ((c) ^ (d) ^ (e)) + (expandedBuffer[i+4]) + 0xca62c1d6); (c) =  ((((c)) << (30)) | (((c)) >> (32-(30)))); };;
	            }
	  			
	            //обновляем итоговое состояние
	            state[0] += a;
	            state[1] += b;
	            state[2] += c;
	            state[3] += d;
	            state[4] += e;
	        }
	  		#endregion
	  	//"Вспомогательные" методы	
	        #region private SHAExpand. Расширяем 16х32 слов в 80х32 слов по принципу x[i] = x[i-3] ^ x[i-8] ^ x[i-14] ^ x[i-16]
	        private static unsafe void SHAExpand (uint* x)
	        {
	            int  i;
	            uint tmp;
	 
	            for (i = 16; i < 80; i++) {
	                tmp =  (x[i-3] ^ x[i-8] ^ x[i-14] ^ x[i-16]);
	                x[i] =  ((tmp << 1) | (tmp >> 31));
	            }
	        }
			#endregion
			#region private DWORDToBigEndian. Преобразуем к порядку big endian
	        internal unsafe static void DWORDToBigEndian (byte[] block, uint[] x, int digits) 
	        {
	            int i;
	            int j;
	  
	            for (i = 0, j = 0; i < digits; i++, j += 4) {
	                block[j] = (byte)((x[i] >> 24) & 0xff);
	                block[j+1] = (byte)((x[i] >> 16) & 0xff);
	                block[j+2] = (byte)((x[i] >> 8) & 0xff);
	                block[j+3] = (byte)(x[i] & 0xff);
	            }
	        }
	        #endregion
	 		#region private DWORDFromBigEndian. Приводим к порядку little endian
	        internal unsafe static void DWORDFromBigEndian (uint* x, int digits, byte* block) 
	        {
	            int i;
	            int j;
	 
	            for (i = 0, j = 0; i < digits; i++, j += 4)
	                x[i] = (uint)((block[j] << 24) | (block[j + 1] << 16) | (block[j + 2] << 8) | block[j + 3]);
        	}
	        #endregion
		#endregion
	}
}
