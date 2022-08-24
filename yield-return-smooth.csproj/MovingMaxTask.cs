using System;
using System.Collections.Generic;
using System.Linq;

namespace yield
{
	public static class MovingMaxTask
	{
		public static void AddToList(LinkedList<double> listOfMax, IEnumerator<DataPoint> enumData)
        {
			while (listOfMax.Last != null && enumData.Current.OriginalY > listOfMax.Last.Value)
				listOfMax.RemoveLast();
			listOfMax.AddLast(enumData.Current.OriginalY);
		}

		public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
		{
			var listOfMax = new LinkedList<double>();
			var listOfWindow = new Queue<double>();
			var enumData = data.GetEnumerator();
			if (!enumData.MoveNext())
				yield break;
			yield return enumData.Current.WithMaxY(enumData.Current.OriginalY);
			listOfMax.AddLast(enumData.Current.OriginalY);
			listOfWindow.Enqueue(enumData.Current.OriginalY);
			int counter = 1;
			while (enumData.MoveNext())
			{
				listOfWindow.Enqueue(enumData.Current.OriginalY);
				AddToList(listOfMax, enumData);
				if (counter < windowWidth)
					counter++;
				else
					listOfMax.Remove(listOfWindow.Dequeue());
				yield return enumData.Current.WithMaxY(listOfMax.First.Value);
			}
		}
	}
}