using System;

namespace Rectangles
{
	public static class RectanglesTask
	{
		// Пересекаются ли два прямоугольника (пересечение только по границе также считается пересечением)
		public static bool AreIntersected(Rectangle r1, Rectangle r2)
		{
			// так можно обратиться к координатам левого верхнего угла первого прямоугольника: r1.Left, r1.Top
			bool hmm = true;
			if ((r1.Bottom < r2.Top || r2.Bottom < r1.Top) || (r1.Left > r2.Right || r2.Left > r1.Right)) hmm = false;
			return hmm;
		}

		// Площадь пересечения прямоугольников
		public static int IntersectionSquare(Rectangle r1, Rectangle r2)
		{
			bool hmm = true;
				if ((r1.Bottom < r2.Top || r2.Bottom < r1.Top) || (r1.Left > r2.Right || r2.Left > r1.Right)) hmm = false;
			int yMin = Math.Max(r1.Top, r2.Top);
			int yMax = Math.Min(r1.Bottom, r2.Bottom);
			int xMin = Math.Max(r1.Left, r2.Left);
			int xMax = Math.Min(r1.Right, r2.Right);
			return hmm ? (xMax - xMin)* (yMax - yMin) : 0;
		}

		// Если один из прямоугольников целиком находится внутри другого — вернуть номер (с нуля) внутреннего.
		// Иначе вернуть -1
		// Если прямоугольники совпадают, можно вернуть номер любого из них.
		public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
		{
			int hmm = -1;
			if ((r1.Bottom <= r2.Bottom && r1.Top >= r2.Top) && (r1.Left >= r2.Left && r1.Right <= r2.Right)) hmm = 0;
			if ((r2.Bottom <= r1.Bottom && r2.Top >= r1.Top) && (r2.Left >= r1.Left && r2.Right <= r1.Right)) hmm = 1;
			return hmm;
		}
	}
}