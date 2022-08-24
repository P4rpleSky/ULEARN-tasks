using System;

namespace Recognizer
{

    internal static class SobelFilterTask
    {
        public static double[,] GetNeighborhood (double[,] g, double[,] sx, int x, int y)
        {
            int width = sx.GetLength(0);
            int height = sx.GetLength(1);
            var result = new double[width, height];
            int i = x;
            int j = y;
            for (int n = i - width / 2; n <= i + width / 2; n++)
                for (int m = j - height / 2; m <= j + height / 2; m++)
                    result[n - i + width / 2, m - j + height / 2] = g[n, m];
            return result;
        }

        public static double[,] SobelFilter(double[,] g, double[,] sx)
        {
            var width = g.GetLength(0);
            var height = g.GetLength(1);
            var widthSX = sx.GetLength(0);
            var heightSX = sx.GetLength(1);
            var result = new double[width, height];
            for (int x = widthSX / 2; x < width - widthSX / 2; x++)
                for (int y = heightSX / 2; y < height - heightSX / 2; y++)
                {
                    var neighbour = GetNeighborhood(g, sx, x, y);
                    double gx = 0, gy = 0;
                    for (int i = 0; i < widthSX; i++)
                        for(int j = 0; j < heightSX; j++)
                        {
                            gx += sx[i, j] * neighbour[i, j];
                            gy += sx[j, i] * neighbour[i, j];
                        }
                    result[x, y] = Math.Sqrt(gx * gx + gy * gy);
                }
            return result;
        }
    }
}