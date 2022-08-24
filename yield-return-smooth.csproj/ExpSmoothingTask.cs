using System.Collections.Generic;

namespace yield
{
	public static class ExpSmoothingTask
	{
		public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
		{
			var enumData = data.GetEnumerator();
			if (!enumData.MoveNext())
				yield break;
			yield return enumData.Current.WithExpSmoothedY(enumData.Current.OriginalY);
			double expSmoothedY = enumData.Current.OriginalY;
			while (enumData.MoveNext())
            {
				expSmoothedY = alpha * enumData.Current.OriginalY + (1 - alpha) * expSmoothedY;
				yield return enumData.Current.WithExpSmoothedY(expSmoothedY);
			}
		}
	}
}