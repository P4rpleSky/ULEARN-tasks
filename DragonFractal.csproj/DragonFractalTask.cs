using System;
using System.Drawing;

namespace Fractals
{
	internal static class DragonFractalTask
	{
		public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
		{
			var random = new Random(seed);
			double x = 1.0;
			double y = 0.0;
			var angle = Math.PI / 4.0;
			for (int i = 0; i < iterationsCount; i++)
            {
				var nextNumber = random.Next(2);
				if (nextNumber == 1)
                {
					double x1 = (x * Math.Cos(angle) - y * Math.Sin(angle))/Math.Sqrt(2.0);
					double y1 = (x * Math.Sin(angle) + y * Math.Cos(angle)) / Math.Sqrt(2.0);
					x = x1;
					y = y1;
					
				}
                else
                {
					double x1 = (x * Math.Cos(3.0*angle) - y * Math.Sin(3.0*angle)) / Math.Sqrt(2.0) + 1.0;
					double y1 = (x * Math.Sin(3.0*angle) + y * Math.Cos(3.0*angle)) / Math.Sqrt(2.0);
					x = x1;
					y = y1;
				}
				pixels.SetPixel(x, y);
			}
		}
	}
}