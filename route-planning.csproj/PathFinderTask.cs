using System;
using System.Collections.Generic;
using System.Drawing;

namespace RoutePlanning
{
    public static class PathFinderTask
    {
        static double BestLength = double.MaxValue;
        static List<int> bestOrder = new List<int>();

        public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
        {
            MakePermutations(checkpoints, new int[checkpoints.Length], 1);
            BestLength = double.MaxValue;
            return bestOrder.ToArray();
        }

        static void Evaluate(Point[] checkpoints, int[] permutation)
        {
            BestLength = 0;
            for (int j = 1; j < permutation.Length; j++)
                BestLength += checkpoints[permutation[j - 1]].DistanceTo(checkpoints[permutation[j]]);
            bestOrder.Clear();
            foreach (var e in permutation)
                bestOrder.Add(e);
        }

        static void CheckForEnd(Point[] checkpoints, int[] permutation, int position)
        {
            double length = 0;
            if (position > 0)
                for (int j = 1; j <= position; j++)
                    length += checkpoints[permutation[j - 1]].DistanceTo(checkpoints[permutation[j]]);
            if (length > BestLength)
                return;
        }

        static void MakePermutations(Point[] checkpoints, int[] permutation, int position)
        {
            if (position == permutation.Length)
            {
                double length = 0;
                for (int j = 1; j < permutation.Length; j++)
                   length += checkpoints[permutation[j - 1]].DistanceTo(checkpoints[permutation[j]]);
                if (length < BestLength)
                    Evaluate(checkpoints, permutation);
                return;
            }
            for (int i = 0; i < permutation.Length; i++)
            {
                var index = Array.IndexOf(permutation, i, 0, position);
                if (index != -1)
                    continue;
                permutation[position] = i;
                CheckForEnd(checkpoints, permutation, position);
                MakePermutations(checkpoints, permutation, position + 1);
            }
        }
    }
}