using System;
using System.IO;
using System.Text;

namespace Sha1Task
{
	/// <summary>
	/// Класс работы с текстовыми файлами
	/// </summary>
	public class TxtFileHandler
	{
		#region Поля
		#endregion
		
		#region Конструктор
		public TxtFileHandler()
		{
		}
		#endregion
		
		#region Методы
			#region public SaveToFile. Сохраняет переданный текст в файл
			public static bool SaveToFile(string text)
			{
				return SaveToFile(text, Encoding.GetEncoding(1251));
			}
			public static bool SaveToFile(string text, Encoding enc)
			{
				bool result = false;
				string filePath = String.Empty;
				
				if (text == null)
					text = String.Empty;
				
				filePath = GetOutputFilePath(); //получим от юзера путь до файла сохранения
				
				if (!String.IsNullOrEmpty(filePath))
				{
					File.WriteAllText(filePath, text, enc);
					result = true;
				}
				
				return result;
			}
			#endregion
			#region public LoadFromFile. Возвращает содержимое выбранного пользователем файла
			public static string LoadFromFile()
			{
				return LoadFromFile(Encoding.GetEncoding(1251));
			}
			public static string LoadFromFile(Encoding enc)
			{
				string result = null;
				
				string filePath = GetInputFilePath(); //получим путь до файла с входными данными
					
				if (!String.IsNullOrEmpty(filePath))
				{
					if (File.Exists(filePath))
					{
						result = File.ReadAllText(filePath, enc); //считываем содержимое файла
					}
				}
				
				return result;
			}
			#endregion
			
			#region private GetOutputFilePath. Вызывает окно выбора файла и возвращает путь к выбранному пользователем файлу
			/// <summary>
			/// Вызывает окно выбора файла и возвращает путь к выбранному пользователем файлу
			/// </summary>
			/// <returns>Путь до файла, в который надо сохранить итоговые данные</returns>
			private static string GetOutputFilePath()
			{
				string result = String.Empty;
				
				System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
		    		
				saveFileDialog.AddExtension = true; //если пользователь не укажет расширение, то оно добавится автоматически
				saveFileDialog.DefaultExt = "txt"; //устанавливаем расширение по умолчанию
				saveFileDialog.Filter =	"Text files (*.txt)|*.txt"; //показываем только файлы txt
				
				if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					result = saveFileDialog.FileName;
				
				return result;
			}
			#endregion
			#region private GetInputFilePath. Вызывает окно выбора файла и возвращает путь к выбранному пользователем файлу
			/// <summary>
			/// Вызывает окно выбора файла и возвращает путь к выбранному пользователем файлу
			/// </summary>
			/// <returns>Путь до файла с входными данными</returns>
			private static string GetInputFilePath()
			{
				string result = String.Empty;
				
				System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
				
				openFileDialog.AddExtension = true; //если пользователь не укажет расширение, то оно добавится автоматически
				openFileDialog.DefaultExt = "txt"; //устанавливаем расширение по умолчанию
				openFileDialog.Filter =	"Text files (*.txt)|*.txt"; //показываем только файлы txt
				//openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //директория которая откроется по умолчанию
				openFileDialog.CheckFileExists = true; //если введенного имени файла не будет - диалоговое окно выдаст предупреждение
				
				if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					result = openFileDialog.FileName;
				
				return result;
			}
			#endregion
			
		#endregion
	}
}
