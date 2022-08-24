using System;

namespace Names
{
    internal static class HeatmapTask
    {
        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
        {
            var birthsCounts = new double[30,12];
            var days = new string[30];
            var months = new string[12];
            for (var y = 0; y < days.Length; y++)
                days[y] = (y + 2).ToString();
            for (var j = 0; j < months.Length; j++)
                months[j] = (j + 1).ToString();
            foreach (var name in names)
                if(name.BirthDate.Day != 1)
                    birthsCounts[name.BirthDate.Day - 2, name.BirthDate.Month - 1]++;                 
            return new HeatmapData(
                "Карта интенсивностей",
                birthsCounts,
                days, 
                months);
        }
    }
}