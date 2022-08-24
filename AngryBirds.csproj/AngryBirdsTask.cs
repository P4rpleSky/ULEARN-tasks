using System;

namespace AngryBirds
{
	public static class AngryBirdsTask
	{
		/// <param name="v">Начальная скорость</param>
		/// <param name="distance">Расстояние до цели</param>
		/// <returns>Угол прицеливания в радианах от 0 до Pi/2</returns>
		public static double FindSightAngle(double v, double distance)
		{
			double g = 9.8;
			double angle = 1.0 / 2.0 * Math.Asin(g * distance / Math.Pow(v, 2));
			return angle;
		}
	}
}