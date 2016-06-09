using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Text;

namespace Sha1Task
{
	/// <summary>
	/// Description of Sha1Form.
	/// </summary>
	public partial class Sha1Form : Form
	{
		#region Константы
		private const int 	cMinHashBitCount = 1,
							cMaxHashBitCount = 160;
		#endregion
		
		#region Поля
		private Sha1Alg _Sha1Main; //для поиска основного хеша
		private Sha1Hash _MainHash; //основной хеш тут
		private string _MainText; //основной текст(от которого берем хеш)
		
		private Thread _FindThread; //поток поиска коллизий
		
		private bool _NeedStopSearch = false; //указывает доп.потоку что надо остановить поиск
		private bool _Sha1Calculated = false; //указывает что основной хеш найден и можно начинать поиск коллизий
		
		private TextBox txtTip = new TextBox(); //хак, нужен для отображения нескольких строк в statusStrip'e
		private int _DefaultStatusStripHeight;
		#endregion
		
		#region Свойства
			#region public int HashBitCount. Число значимых бит в хеше
			/// <summary>
			/// Число значимых бит в хеше
			/// </summary>
			public int HashBitCount
			{
				set{
					if (_Sha1Main != null)
					{
						_Sha1Main.HashBitCount = value;
						lHashCount.Text = String.Format(Str.cHashCountText, value.ToString());
						lColliseHashCount.Text = lHashCount.Text;
					}
					else
						ShowStatusMessage(Str.cCanNotSetHashBitCount, true);
				}
				get{
					if (_Sha1Main != null)
						return _Sha1Main.HashBitCount;
					else
						return -1;
				}
			}
			#endregion
		#endregion
		
		#region Конструктор
		public Sha1Form()
		{
			InitializeComponent();
		}
		#endregion
		
		#region Методы
			#region Init. Первичная инициализация
			/// <summary>
			/// Первичная инициализация
			/// </summary>
			private void Init()
			{
				try
				{
					_Sha1Main = new Sha1Alg(); //класс, через который будем работать с хешами в осн.потоке
					tbHashBitCount.Text = HashBitCount.ToString();
					HashBitCount = HashBitCount;
					
					bFindCollision.Tag = true; //устанавливаем начальное состояние кнопки в "Найти коллизию"
					bFindCollision.Text = Str.cFindCollisionText; //Начальный текст на кнопке
					
					CollisionViewReset();
					
					_DefaultStatusStripHeight = statusStrip1.Height;
					txtTip.Multiline = true;
					txtTip.WordWrap = true;
					txtTip.Font = statusLabel.Font;//такой же шрифт как и у лейблы
					
					statusStrip1.BackColor = this.BackColor;
				}
				catch(Exception ex)
				{
					ShowStatusMessage(Str.cExceptionHappened + ex.Message, true);
				}
			}
			#endregion
			#region ShowStatusMessage. Выводит сообщение в статусную строку
			/// <summary>
			/// Очищает статусную строку
			/// </summary>
			private void ShowStatusMessage()
			{
				ShowStatusMessage(String.Empty);
			}
			/// <summary>
			/// Выводит сообщение в статусную строку
			/// </summary>
			/// <param name="text">Текст сообщения</param>
			private void ShowStatusMessage(string text)
			{
				ShowStatusMessage(text, false);
			}
			/// <summary>
			/// Выводит сообщение в статусную строку
			/// </summary>
			/// <param name="text">Текст сообщения</param>
			/// <param name="isError">Указывает, передано сообщение об ошибке или нет</param>
			private void ShowStatusMessage(string text, bool isError)
			{
				try
				{
					if (!String.IsNullOrEmpty(text))
						statusLabel.Tag = text.Replace("\n", " ").Replace(Environment.NewLine, " ");
					else
						statusLabel.Tag = String.Empty;
					
					
					if (isError)
						statusLabel.ForeColor = Color.Red;
					else
						statusLabel.ForeColor = Color.Black;
					
					
					statusStrip1.Refresh();
				}
				catch(Exception ex)
				{
					MessageBox.Show(Str.cExceptionHappened + ex.Message);
				}
			}
			#endregion
			
			#region CollisionViewReset. Сбрасываем информацию о найденных коллизиях на форме
			private void CollisionViewReset()
			{
				try
				{
					tbCollisionText.Text = String.Empty;
					lCollisionHash.Text = "0x00";
					lCollisionTime.Text = "00:00:00.0000";
					lTruncCollisionHash.Text = "0x00";
					lAttemptCount.Text = "0";
				}
				catch (Exception ex)
				{
					MessageBox.Show(Str.cExceptionHappened + ex.Message);
				}
			}
			#endregion
			
			#region GetRandomString. Получает случайную строку с заданной длинной
			private string GetRandomString(int length)
			{
				StringBuilder result = new StringBuilder();
				
				if (length > 0)
				{
					Random random = new Random(DateTime.Now.Second+DateTime.Now.Millisecond);
					
					for (int i=0; i<length; i++)
					{
						result.Append((char)random.Next(1, 255)); //работаем в диапазоне win-1251
					}
				}
				
				return result.ToString();
			}
			#endregion
			#region FindCollision. Ищет коллизию. Запускается в доп.потоке
			private void FindCollision()
			{
				try
				{
					//заведем константы:
					const int 	cMinTextLength = 1, //минимальная длина случайной строки
								cMaxTextLength = 512, //максимальная - чтобы не улететь в переполнение
								cNextAttemptCount = 300; //через каждое такое количество попыток - производим изменение длины строки
					
					
					//зафиксируем копии:			
					string mainText = String.Copy(_MainText);
					Sha1Hash mainHash = _MainHash.Clone(); //возьмем копию
					
					//заведем необходимые переменные:
					Sha1Alg sha1Add = new Sha1Alg(); //заведем отдельный класс, чтобы не заморачиваться с локами
					sha1Add.HashBitCount = _Sha1Main.HashBitCount; //устанавливаем количество значимых бит
					Sha1Hash addHash = new Sha1Hash(); //тут будет хеш случайных строк
					
					string randomText = String.Empty; //тут будет случайная строка
					Stopwatch hashSW = new Stopwatch(); //время поиска
					int attemptCount = 0; //количество попыток
					bool isFinded = false; //нашли\не нашли
					
					bool upWay = true; //идея в том, чтобы до определенного значения увеличивать длину строки, по достижении его - уменьшать. и потом по-новой
					int oldNextAttemptValue = 0;
					
					int textLength = cMinTextLength; //установим начальную длину случайной строки
					hashSW.Start(); //запускаем замер времени поиска
					
					//поиск:
					while (!_NeedStopSearch && !isFinded)
					{
						randomText = GetRandomString(textLength); //получаем случайную строку
						
						addHash = sha1Add.GetHash(randomText);
						
						//если усеченные хеши совпали и при этом не совпали строки, от которых они взяты
						if (sha1Add.Compare(mainHash, addHash) && mainText.IndexOf(randomText) < 0)
							isFinded = true; //коллизия найдена
						
												
						if (attemptCount++ / cNextAttemptCount > oldNextAttemptValue) //если пересекли очередной рубеж для изменения длины строки
						{
							oldNextAttemptValue = attemptCount/cNextAttemptCount;
							
							if (upWay) //если сейчас режим увеличения случайной строки
							{
								if (++textLength >= cMaxTextLength)
									upWay = !upWay; //меняем направление
							}
							else
							{
								if (--textLength <= cMinTextLength)
									upWay = !upWay; //меняем направление
							}
							
							//для подобия обратной связи - обновляем показатели времени поиска и количества попыток
							//надо конечно делать через таймер, но и это лучше чем ничего:
							Invoke(new Action( delegate()
						    {
			                  	lCollisionTime.Text = hashSW.Elapsed.ToString();
								lAttemptCount.Text = attemptCount.ToString();
						    }));
						}
						
					}
					
					hashSW.Stop();
					
					//выходим в основной поток, переводим в состояние завершения поиска
					Invoke(new Action( delegate()
				    {
						bFindCollision.Tag = true;
						bFindCollision.Text = Str.cFindCollisionText;
						
						if (isFinded) //если нас не прервали и коллизия найдена
						{
							tbCollisionText.Text = randomText;
							lCollisionHash.Text = "0x"+addHash.Text;
							lCollisionTime.Text = hashSW.Elapsed.ToString();
							lTruncCollisionHash.Text = "0x"+_Sha1Main.GetTruncateHash(addHash.Value).Text;
							lAttemptCount.Text = attemptCount.ToString();
							
							ShowStatusMessage(Str.cCollisionFinded);
						}
				    }));
				}
				catch
				{
					//в releise-версии тут нечего ловить, т.к. при завершении потока постоянно будут сообщения
				}
			}
			#endregion
		#endregion
		
		#region Обработчики событий
			#region OnBLoadFromFileClick. Кнопка "Загрузить из файла"
			void OnBLoadFromFileClick(object sender, EventArgs e)
			{
				try
				{
					string loadData = TxtFileHandler.LoadFromFile();
					
					if (loadData != null) //если какие-либо данные были подгружены
					{
						tbInputText.Text = loadData;
						ShowStatusMessage(Str.cDataLoaded);
					}
					else
						ShowStatusMessage(Str.cDataNotLoaded);
				}
				catch(Exception ex)
				{
					ShowStatusMessage(Str.cExceptionHappened + ex.Message, true);
				}
			}
			#endregion
			#region OnBSaveToFileClick. Кнопка "Сохранить в файл"
			void OnBSaveToFileClick(object sender, EventArgs e)
			{
				try
				{
					if (TxtFileHandler.SaveToFile(tbCollisionText.Text)) //если сохранение произошло
						ShowStatusMessage(Str.cDataSaved);
					else
						ShowStatusMessage(Str.cDataNotSaved);
				}
				catch(Exception ex)
				{
					ShowStatusMessage(Str.cExceptionHappened + ex.Message, true);
				}
			}
			#endregion	
		
			#region OnBCalcSha1Click. Кнопка "Вычислить SHA1-хеш"
			void OnBCalcSha1Click(object sender, EventArgs e)
			{
				try
				{
					ShowStatusMessage(Str.cHashCalculating);
					
					if (_Sha1Main == null)
						_Sha1Main = new Sha1Alg();
					
					//получаем количество значимых бит от пользователя и если оно корректно устанавливаем его, 
					//иначе - работаем с количеством бит по умолчанию
					int userHashBitCount;
					
					if (int.TryParse(tbHashBitCount.Text, out userHashBitCount)
					    && userHashBitCount >= cMinHashBitCount && userHashBitCount <= cMaxHashBitCount)
						HashBitCount = userHashBitCount;
					else
						tbHashBitCount.Text = HashBitCount.ToString();
					
					Stopwatch hashSW = new Stopwatch();
					hashSW.Start(); //запускаем замер времени выполнения
					
					_MainHash = _Sha1Main.GetHash(tbInputText.Text); //вычисляем хеш от содержимого текстбокса
					hashSW.Stop(); //останавливаем, независимо от результата
					
					if (_MainHash != null && !String.IsNullOrEmpty(_MainHash.Text))
					{
						lSha1Hash.Text = "0x"+_MainHash.Text;
						lTruncSha1Hash.Text = "0x"+_Sha1Main.GetTruncateHash(_MainHash.Value).Text;
						lSha1Time.Text = hashSW.Elapsed.ToString();
						
						_MainText = tbInputText.Text;
						_Sha1Calculated = true; //указываем что в _Sha1Main посчитанный хеш и можно искать коллизии
						
						ShowStatusMessage(Str.cHashCalcEnd);
					}
					else
						ShowStatusMessage(Str.cCantCalcHash, true);
					
				}
				catch(Exception ex)
				{
					ShowStatusMessage(Str.cExceptionHappened + ex.Message, true);
				}
			}
			#endregion
			#region OnBFindCollisionClick. Кнопка "Найти коллизию"\"Остановить поиск"
			void OnBFindCollisionClick(object sender, EventArgs e)
			{
				try
				{
					Button bSender = sender as Button;
					bool findMode = (bool) bSender.Tag; //получаем через таг состояние кнопки(найти\остановить)
					
					if (findMode) //кнопка нажата в состоянии "Найти"
					{
						if (_Sha1Calculated) //если уже найден основной хеш, относительно которого будем искать коллизии
						{
							ShowStatusMessage(Str.cSearching); //уведомляем пользователя о начале поиска
							_NeedStopSearch = false; //необходимость остановить поток отсутствует
							
							CollisionViewReset(); //сбрасываем отображение параметров коллизии
													
							//в новом потоке запускаем поиск коллизий
							_FindThread = new Thread(FindCollision);
							_FindThread.IsBackground = true;
							_FindThread.Start();
							
							bSender.Tag = !findMode; //запоминаем новое состояние
							bSender.Text = Str.cStopSearchText; //меняем текст на кнопке
						}
						else
							ShowStatusMessage(Str.cNeedHash, true);
					}
					else //кнопка нажата в состоянии "Остановить поиск"
					{
						_NeedStopSearch = true; //оповещаем поток поиска о необходимости остановки
						
						//немного ждем завершения потока и если не отвечает, прерываем принудительно
						if (_FindThread != null && _FindThread.IsAlive)
						{
							_FindThread.Join(600);
	        				
	        				if (_FindThread.IsAlive)
	        					_FindThread.Abort();
						}
						
						CollisionViewReset(); //т.к. поиск незавершен - сбрасываем отображение параметров коллизии
						
						bSender.Tag = !findMode; //запоминаем новое состояние
						bSender.Text = Str.cFindCollisionText; //меняем текст на кнопке
						
						ShowStatusMessage(Str.cSearchStopped); //уведомляем пользователя о прерывании поиска
					}
					
				}
				catch(Exception ex)
				{
					ShowStatusMessage(Str.cExceptionHappened + ex.Message, true);
				}
			}
			#endregion
			
			#region OnSha1FormLoad. При загрузке формы проводим первичную инициализацию
			void OnSha1FormLoad(object sender, EventArgs e)
			{
				Init();
			}
			#endregion
			#region OnSha1FormFormClosing. Перед закрытием формы остановим поиск коллизий
			void OnSha1FormFormClosing(object sender, FormClosingEventArgs e)
			{
				try
				{	
					if (!(bool) bFindCollision.Tag) //Если сейчас в поиске коллизии, то прекращаем поиск
						OnBFindCollisionClick(bFindCollision, null); //т.к. иначе при выходе в меню поток поиска продолжит работу
				}
				catch{}
			}
			#endregion
			#region OnStatusLabel_Paint. Отслеживаем перерисовку статусной строки и корректируем невлезающий текст
			private void OnStatusLabel_Paint(object sender, PaintEventArgs e)
			{
				try
				{
					String textToPaint = statusLabel.Tag != null ? statusLabel.Tag.ToString().Replace("\n", " ")
						: String.Empty; //получаем строку через Tab
					
					SizeF stringSize = e.Graphics.MeasureString(textToPaint, statusLabel.Font);
					
					if (stringSize.Width > (statusStrip1.Width - 10))//если строка шире чем надо, разобьем на несколько
					{
						//используем текстбокс, чтобы узнать сколько строк нам потребуется
						txtTip.Height = statusStrip1.Height;
						txtTip.Width = statusStrip1.Width - 10;//statusLabel.Width - 10;
						txtTip.Text = textToPaint;
						
						int linesRequired = txtTip.GetLineFromCharIndex(textToPaint.Length - 1) + 1;
						
						statusStrip1.Height =((int)stringSize.Height * linesRequired) + 5;
						
						statusLabel.Text = "";
						
						e.Graphics.DrawString(textToPaint, statusLabel.Font, new SolidBrush( statusLabel.ForeColor), new RectangleF( new PointF(0, 0), new SizeF(statusStrip1.Width, statusStrip1.Height)));
					}
					else
					{
						statusStrip1.Height = _DefaultStatusStripHeight;
						statusLabel.Text = textToPaint;
					}
				}
				catch(Exception ex)
				{
					ShowStatusMessage(Str.cExceptionHappened + ex.Message, true);
				}
			}
			#endregion
		#endregion
		
			
	}
}
