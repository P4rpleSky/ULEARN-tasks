using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.PairsAnalysis
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> data)
        {
            var enumerator = data.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new InvalidOperationException();
            var past = enumerator.Current;
            if (!enumerator.MoveNext())
                throw new InvalidOperationException();
            yield return Tuple.Create(past, enumerator.Current);
            past = enumerator.Current;
            while (enumerator.MoveNext())
            {
                yield return Tuple.Create(past, enumerator.Current);
                past = enumerator.Current;
            }
        }

        public static int MaxIndex<T>(this IEnumerable<T> data)
            where T : IComparable
        {
            var enumerator = data.GetEnumerator();
            enumerator.MoveNext();
            var past = enumerator.Current;
            int maxIndex = 0;
            int currentIndex = 1;
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.CompareTo(past) >= 0)
                    maxIndex = currentIndex;
                past = enumerator.Current;
                currentIndex++;
            }
            return maxIndex;
        }
    }
    
    public static class Analysis
    {
        public static int FindMaxPeriodIndex(params DateTime[] data)
        {
            return data.Pairs<DateTime>()
                .Select(pair => (pair.Item2 - pair.Item1).TotalSeconds)
                .MaxIndex();
        }

        public static double FindAverageRelativeDifference(params double[] data)
        {
            return data.Pairs<double>()
                .Average(pair => (pair.Item2 - pair.Item1) / pair.Item1);
        }
    }
}
