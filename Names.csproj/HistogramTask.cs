using System;
using System.Linq;

namespace Names
{
    internal static class HistogramTask
    {
        public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
        {
            var days = new string[31];
            for (var y = 0; y < days.Length; y++)
                days[y] = (y + 1).ToString();
            var birthsCounts = new double[31];
            foreach (var x in names)
            {
                if (x.Name == name && x.BirthDate.Day != 1)
                    birthsCounts[x.BirthDate.Day - 1]++;
            }
            return new HistogramData(
                string.Format("Рождаемость людей с именем '{0}'", name),
                days,
                birthsCounts);
        }
    }
}