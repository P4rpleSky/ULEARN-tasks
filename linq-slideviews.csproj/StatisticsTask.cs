using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class StatisticsTask
	{
		public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
		{
            var timesOfUsers = visits.GroupBy(x => x.UserId)
                .Select(x => x.OrderBy(y => y.DateTime).Bigrams())
                .SelectMany(x => x.Where(y => Enum.Equals(y.Item1.SlideType, slideType))
                                  .Select(y => (y.Item2.DateTime - y.Item1.DateTime).TotalMinutes)
                                  .Where(y => y >= 1 && y <= 120));
            return timesOfUsers.Any() ? timesOfUsers.Median() : 0;
        }
    }
}