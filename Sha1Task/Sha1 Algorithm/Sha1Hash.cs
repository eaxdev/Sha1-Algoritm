using System;
using System.Text;

namespace Sha1Task
{
	/// <summary>
	/// Описывает sha1-хеш
	/// </summary>
	public class Sha1Hash
	{
		#region Поля и константы
		private const int 	cByteCountDefault = 20, //количество байт хеша
							cGroupCount = 8; //количество байт в группе, каждая группа обособленна от других пробелом
		
		private int _ByteCount = cByteCountDefault;			
		private byte[] _Value = null;
		#endregion
		
		#region Свойства
			#region public ByteCount. Количество байт в хеше
			/// <summary>
			/// Количество полных байт в хеше
			/// </summary>
			public int ByteCount
			{
				set{
					_ByteCount = value;
				}
				get{
					return _ByteCount;
				}
			}
			#endregion
			#region public Value. Значение хеша
			/// <summary>
			/// Значение хеша
			/// </summary>
			public byte[] Value
			{
				set{
					_Value = value;
				}
				get{
					return _Value;
				}
			}
			#endregion
			#region public Text. Текстовое значение хеша
			/// <summary>
			/// Текстовое значение хеша
			/// </summary>
			public string Text
			{
				get{
					if (Value != null)
					{
						string sHash = BitConverter.ToString(Value).Replace("-", "");
						
						if (sHash.Length > cGroupCount)
						{ //если общее количество байт не влазит в одну группу
							StringBuilder sb = new StringBuilder(sHash, 0, cGroupCount, ByteCount*2+ByteCount/cGroupCount+1);
							sb.Append(' '); //первый разделитель сразу за группой
							
							int num = cGroupCount;
							for (int i=cGroupCount; i<sHash.Length; i++)
							{
								sb.Append(sHash[i]);
								
								if ((num++ + 1)%cGroupCount == 0)
									sb.Append(' '); //после каждой группы ставим разделитель
							}
							
							sHash = sb.ToString();
						}
						
						return sHash;
					}
					else
						return String.Empty;
				}
			}
			#endregion
		#endregion
		
		#region Конструктор
		public Sha1Hash(): this(null)
		{ }
		public Sha1Hash(byte[] Value)
		{
			this.Value = Value;			
		}
		#endregion
		
		#region Clone. Возвращает копию данного экземпляра класса
		public Sha1Hash Clone()
		{
			Sha1Hash result = new Sha1Hash();
			
			result.ByteCount = this.ByteCount;
			
			if (this.Value != null)
			{
				result.Value = new byte[this.Value.Length];
				this.Value.CopyTo(result.Value, 0);
			}
			
			return result;
		}		
		#endregion
	}
}
