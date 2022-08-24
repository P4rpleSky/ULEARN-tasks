using System;

namespace DistanceTask
{
	public static class DistanceTask
	{
		// Расстояние от точки (x, y) до отрезка AB с координатами A(ax, ay), B(bx, by)
		public static double getX (double x1, double y1, double x2, double y2, double a, double b)
        {
			double k, m, xProj;
			k = (y1 - y2) / (x1 - x2);
			m = y1 - k * x1;
			if (k == 0) xProj = a;
			else xProj = (b + a / k - m) / (k + 1 / k);
			return xProj;
		}
		public static double getY (double x1, double y1, double x2, double y2, double a, double b)
		{
			double k, m, yProj;
			k = (y1 - y2) / (x1 - x2);
			m = y1 - k * x1;
			if (k == 0) yProj = b;
			else yProj = k * (b + a / k - m) / (k + 1 / k) + m;
			return yProj;
		}

		public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
		{
			//Прямая - точка
			if (ax == bx && ay == by) return Math.Sqrt((ax - x) * (ax - x) + (ay - y) * (ay - y));

			//Прямая - НЕ точка
			double k, m, distance;
			//Не вертикальная
			if (bx != ax)
			{
				k = (by - ay) / (bx - ax);
				m = by - k * bx;
				distance = Math.Abs(y - k * x - m) / Math.Sqrt(1.0 + k * k);

				//Точка падения
				double xxx = getX(ax, ay, bx, by, x, y);
				double yyy = getY(ax, ay, bx, by, x, y);

				//Сама точка не принадлежит прямой, а проекция принадлежит отрезку
				if ((y - k * x - m != 0) && (xxx >= Math.Min(ax, bx) && xxx <= Math.Max(ax, bx)))
					return distance;
				//Сама точка не принадлежит прямой, а проекция НЕ принадлежит отрезку
				if ((y - k * x - m != 0) && (xxx < Math.Min(ax, bx) || xxx > Math.Max(ax, bx)))
					return Math.Min(Math.Sqrt((ax - x) * (ax - x) + (ay - y) * (ay - y)), Math.Sqrt((bx - x) * (bx - x) + (by - y) * (by - y)));

				//Сама точка принадлежит прямой и принадлежит отрезку
				if ((y - k * x - m == 0) && (x >= Math.Min(ax, bx) && x <= Math.Max(ax, bx)))
					return 0;
				//Сама точка принадлежит прямой и НЕ принадлежит отрезку
				if ((y - k * x - m == 0) && (x < Math.Min(ax, bx) || x > Math.Max(ax, bx)))
					return Math.Min(Math.Sqrt((ax - x) * (ax - x) + (ay - y) * (ay - y)), Math.Sqrt((bx - x) * (bx - x) + (by - y) * (by - y)));
			}
			//Вертикальная
			else
			{
				//Сама точка не принадлежит прямой, а проекция принадлежит отрезку
				if (x != ax && (y >= Math.Min(ay, by) && y <= Math.Max(ay, by)))
					return Math.Abs(x - ax);
				//Сама точка не принадлежит прямой, а проекция НЕ принадлежит отрезку
				if (x != ax && (y < Math.Min(ay, by) || y > Math.Max(ay, by)))
					return Math.Min(Math.Sqrt((ax - x) * (ax - x) + (ay - y) * (ay - y)), Math.Sqrt((bx - x) * (bx - x) + (by - y) * (by - y)));

				//Сама точка принадлежит прямой и принадлежит отрезку
				if (x == ax && (y >= Math.Min(ay, by) && y <= Math.Max(ay, by)))
					return 0;
				//Сама точка принадлежит прямой и НЕ принадлежит отрезку
				if (x == ax && (y < Math.Min(ay, by) || y > Math.Max(ay, by)))
					return Math.Min(Math.Abs(y - ay), Math.Abs(y - by));
			}
			return 0;
		}
	}
}