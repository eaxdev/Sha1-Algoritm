using System;

namespace Sha1Task
{
	/// <summary>
	/// Все строки, использующиеся в программе, находятся в данном классе
	/// </summary>
	public class Str
	{
		#region Сообщения
		public const string	cDataLoaded = "Данные из файла загружены",
							cDataNotLoaded = "Данные не загружены",
							cDataSaved = "Данные сохранены в файл",
							cDataNotSaved = "Данные не сохранены",
							cHashCalculating = "Вычисляем sha1-хеш для входных данных",
							cHashCalcEnd = "Sha1-хеш найден",
							cFindCollisionText = "Найти коллизию",
							cStopSearchText = "Остановить поиск",
							cTestFormCaption = "Sha1Task. Тестирование",
							cTestResultFormCaption = "Sha1Task. Результаты тестирования",
							cTestNumberOf = ". Вопрос {0} из {1}",
							cSearching = "Производится поиск коллизий...",
							cSearchStopped = "Поиск коллизий прерван",
							cCollisionFinded = "Коллизия найдена",
							cHashCountText = "Хеш({0} бит):";
		#endregion
		#region Ошибки
		public const string	cExceptionHappened = "Возникло исключение: ",
							cCantCalcHash = "Не удалось вычислить хеш",
							cNeedHash = "Для начала поиска коллизий, необходимо вычислить основной хеш",
							cArrayCountNotEqual = "Количество вопросов не соответствует количеству ответов",
							cIncorrectQuestionCount = "Количество вопросов для вывода не может превышать общее количество вопросов",
							cCanNotSetHashBitCount = "Невозможно установить количество значимых бит";
		#endregion
		
		public Str()
		{
		}
	}
}
