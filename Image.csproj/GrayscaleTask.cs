namespace Recognizer
{
	public static class GrayscaleTask
	{
		public static double[,] ToGrayscale(Pixel[,] original)
		{
			var x = original.GetLength(0);
			var y = original.GetLength(1);
			var grayscale = new double[x, y];
			for (int i = 0; i < x; i++)
				for (int j = 0; j < y; j++)
                {
					var r = original[i, j].R;
					var g = original[i, j].G;
					var b = original[i, j].B;
					grayscale[i, j] = (0.299 * r + 0.587 * g + 0.114 * b) / 255.0;
				}
			return grayscale;
		}
	}
}