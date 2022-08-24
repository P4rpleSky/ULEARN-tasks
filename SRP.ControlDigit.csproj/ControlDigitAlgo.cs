using System;

namespace SRP.ControlDigit
{
    public static class Extensions
    {
        public static int GetChecksum(this long number, int initFactor, Func<int,int> hashFunc, Func<int,int> factorFunc)
        {
            int checksum = 0;
            do
            {
                int digit = (int)(number % 10);
                checksum += hashFunc(initFactor * digit);
                initFactor = factorFunc(initFactor);
                number /= 10;
            }
            while (number > 0);
            return checksum;
        }

        public static int GetSumOfNumberDigits(this int number)
        {
            int result = 0;
            do
            {
                result += (int)(number % 10);
                number /= 10;
            }
            while (number > 0);
            return result;
        }
        // Вспомогательные методы-расширения поместите в этот класс.
        // Они должны быть понятны и потенциально полезны вне контекста задачи расчета контрольных разрядов.
    }

    public static class ControlDigitAlgo
    {
        public static int Upc(long number)
        {
            var sum = number.GetChecksum(3, x => x, x => 4 - x);

            int result = sum % 10;
            if (result != 0)
                result = 10 - result;
            return result;
        }

        public static char Isbn10(long number)
        {
            var sum = number.GetChecksum(2, x => x, x => x + 1);

            int result = sum % 11;
            if (result != 0)
                result = 11 - result;
            return result == 10? 'X' : char.Parse(result.ToString());
        }

        public static int Luhn(long number)
        {
            var sum = number.GetChecksum( 2, x => x.GetSumOfNumberDigits(), x => 3 - x);

            int result = sum % 10;
            if (result != 0)
                result = 10 - result;
            return result;
        }
    }
}
