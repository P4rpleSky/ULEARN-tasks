using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class ParsingTask
	{
		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
		/// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
		/// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
		public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
		{
			SlideType slideType;
			int slideId;
			Type type = typeof(SlideType);
			return lines.Select(x => x.Split(';'))
				.Where(x => x.Length == 3 && 
					   		int.TryParse(x[0], out slideId) && 
					   		Enum.TryParse(x[1], true, out slideType))
				.ToDictionary(x => int.Parse(x[0]), 
							  x => new SlideRecord(int.Parse(x[0]), (SlideType)Enum.Parse(type, x[1], true), x[2]));
		}

		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
		/// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
		/// Такой словарь можно получить методом ParseSlideRecords</param>
		/// <returns>Список информации о посещениях</returns>
		/// <exception cref="FormatException">Если среди строк есть некорректные</exception>
		public static IEnumerable<VisitRecord> ParseVisitRecords(
			IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
		{
			int userId;
			int slideId;
			DateTime dateTime;
			return lines.Select(x => x.Split(';'))
				.Skip(1)
				.Select(x => x.Length == 4 &&
							 int.TryParse(x[0], out userId) &&
							 int.TryParse(x[1], out slideId) &&
							 DateTime.TryParse(x[2] + "T" + x[3], out dateTime) &&
							 slides.ContainsKey(int.Parse(x[1])) ?
							 x : throw new FormatException(String.Format("Wrong line [{0}]", String.Join(";", x))))
				.Select(x => new VisitRecord(int.Parse(x[0]), int.Parse(x[1]),
											 DateTime.Parse(x[2] + "T" + x[3]),
											 slides[int.Parse(x[1])].SlideType));
		}
	}
}