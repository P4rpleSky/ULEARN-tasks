uusing System;
using System.Collections.Generic;
using System.Linq;

namespace Antiplagiarism
{
    public static class LongestCommonSubsequenceCalculator
    {
        public static List<string> Calculate(List<string> first, List<string> second)
        {
            var opt = CreateOptimizationTable(first, second);
            return RestoreAnswer(opt, first, second);
        }

        private static int[,] CreateOptimizationTable(List<string> first, List<string> second)
        {
            var opt = new int[first.Count + 1, second.Count + 1];
            for (int i = 0; i <= first.Count; i++) opt[i, 0] = 0;
            for (var j = 0; j <= second.Count; ++j) opt[0, j] = 0;

            for (var i = 1; i <= first.Count; ++i)
            {
                for (var j = 1; j <= second.Count; ++j)
                {
                    if (first[i - 1] == second[j - 1])
                        opt[i, j] = opt[i - 1, j - 1] + 1;
                    else
                        opt[i, j] = Math.Max(opt[i, j - 1], opt[i - 1, j]);
                }
            }
            return opt;
        }

        private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
        {
            var result = new List<string>();
            int row = first.Count;
            int column = second.Count;
            while (row != 0 && column != 0)
            {
                if (opt[row, column - 1] == opt[row, column]) column--;
                else if (opt[row - 1, column] == opt[row, column]) row--;
                else
                {
                    result.Add(second[column - 1]);
                    column--;
                    row--;
                }
            }
            result.Reverse();
            return result;
        }
    }
}