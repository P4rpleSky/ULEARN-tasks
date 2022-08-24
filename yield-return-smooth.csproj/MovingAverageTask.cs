using System.Collections.Generic;

namespace yield
{
	public static class MovingAverageTask
	{
		public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
		{
			var queue = new Queue<double>();
			var enumData = data.GetEnumerator();
			if (!enumData.MoveNext())
				yield break;
			yield return enumData.Current.WithAvgSmoothedY(enumData.Current.OriginalY);
			queue.Enqueue(enumData.Current.OriginalY);
			int counter = 1;
			double sum = enumData.Current.OriginalY;
			while (enumData.MoveNext())
			{
				if (counter < windowWidth)
				{
					queue.Enqueue(enumData.Current.OriginalY);
					counter++;
					sum += enumData.Current.OriginalY;
					yield return enumData.Current.WithAvgSmoothedY(sum / counter);
				}
                		else
                		{
					sum += enumData.Current.OriginalY - queue.Dequeue();
					queue.Enqueue(enumData.Current.OriginalY);
					yield return enumData.Current.WithAvgSmoothedY(sum / counter);
				}
			}
		}
	}
}