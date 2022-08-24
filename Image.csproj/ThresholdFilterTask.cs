using System.Collections.Generic;

namespace Recognizer
{
	public static class ThresholdFilterTask
	{
		public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
		{
			var x = original.GetLength(0);
			var y = original.GetLength(1);
			List<double> listOfPixels = new List<double>();
			foreach (var e in original)
				listOfPixels.Add(e);
			listOfPixels.Sort();
			var result = new double[x, y];
			if ((int)(whitePixelsFraction * listOfPixels.Count) < 1) 
				return result;
			int index = listOfPixels.Count - (int)(whitePixelsFraction * listOfPixels.Count);
			if (whitePixelsFraction == 1) 
				index = 0;
			double t = listOfPixels[index];
			for (int i = 0; i < x; i++)
				for (int j = 0; j < y; j++)
				{
					if (original[i, j] >= t)
						result[i, j] = 1.0;
					else
						result[i, j] = 0.0;
				}
			return result;
		}
	}
}