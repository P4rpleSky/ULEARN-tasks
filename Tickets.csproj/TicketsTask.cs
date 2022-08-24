using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tickets
{
    class TicketsTask
    {
        public static BigInteger Solve(int halfLen, int totalSum)
        {
            int halfSum = totalSum / 2;
            if (totalSum % 2 == 1)
                return 0;
            BigInteger halfCount = GetCountOfNumbers(halfLen, halfSum);
            return halfCount * halfCount;
        }

        private static BigInteger GetSumOfLastNine(BigInteger[] vector, int index)
        {
            BigInteger sum = 0;
            int stopIndex = index - 9;
            while(index >= 0 && index >= stopIndex)
            {
                sum += vector[index];
                index--;
            }
            return sum;
        }

        private static BigInteger GetCountOfNumbers(int length, int sum)
        {
            BigInteger[] previous = new BigInteger[sum + 1];
            BigInteger[] current = new BigInteger[sum + 1];

            for (int i = 0; i < current.Length; i++)
                if (i < 10) current[i] = 1;
                else current[i] = 0;

            for (int j = 2; j <= length; j++)
            {
                current.CopyTo(previous, 0);
                current[0] = 1;
                for (int i = 1; i < current.Length; i++)
                    current[i] = GetSumOfLastNine(previous, i);
            }
            return current[current.Length - 1];
        }
    }
}