using System.Collections.Generic;
using System.Linq;

namespace Recognizer
{
	internal static class MedianFilterTask
	{
		public static void AddToList (List<double> list, double[,] original, int i, int j)
        {
			var x = original.GetLength(0);
			var y = original.GetLength(1);
			list.Add(original[i, j]);
			if (j != 0)
				list.Add(original[i, j - 1]);
			if (j != y - 1)
				list.Add(original[i, j + 1]);
			if (i != 0)
				list.Add(original[i - 1, j]);
			if (i != x - 1)
				list.Add(original[i + 1, j]);
			if (j != 0 && i != 0)
				list.Add(original[i - 1, j - 1]);
			if (j != y - 1 && i != x - 1)
				list.Add(original[i + 1, j + 1]);
			if (j != y - 1 && i != 0)
				list.Add(original[i - 1, j + 1]);
			if (j != 0 && i != x - 1)
				list.Add(original[i + 1, j - 1]);
		}
		
		public static double[,] MedianFilter(double[,] original)
		{
			var x = original.GetLength(0);
			var y = original.GetLength(1);
			var result = new double[x, y];
			for (int i = 0; i < x; i++)
				for (int j = 0; j < y; j++)
				{
					List<double> list = new List<double>();
					AddToList (list, original, i, j);
					list.Sort();
					int n = list.Count;
					if (n % 2 == 0)
						result[i, j] = (list[n / 2 - 1] + list[n / 2]) / 2.0;
					else
						result[i, j] = list[n / 2];
				}
			return result;
		}
	}
}