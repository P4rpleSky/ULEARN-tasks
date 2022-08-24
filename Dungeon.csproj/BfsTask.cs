using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
	public static class PointExtensions
    {
		public static IEnumerable<Point> GetNeighboursOnMap(this Point currentPoint, Map map)
        {
			for (int dx = -1; dx < 2; dx++)
				for (int dy = -1; dy < 2; dy++)
					if (Math.Abs(dx - dy) == 1 &&
                        map.InBounds(new Point(currentPoint.X + dx, currentPoint.Y + dy)) &&
                        map.Dungeon[currentPoint.X + dx, currentPoint.Y + dy] == MapCell.Empty)
						yield return new Point(currentPoint.X + dx, currentPoint.Y + dy);
		}
    }
	
	public class BfsTask
	{
        private static SinglyLinkedList<Point> GetPath(Dictionary<Point, SinglyLinkedList<Point>> usedTracks,
            Dictionary<Point, Point> track, Point start, Point end)
        {
            if (usedTracks.ContainsKey(end)) return usedTracks[end];
            if (start == end) return new SinglyLinkedList<Point>(start);
            return new SinglyLinkedList<Point>(end, GetPath(usedTracks, track, start, track[end]));
        }

        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
            int chestsRemain = chests.Length;
            var usedTracks = new Dictionary<Point, SinglyLinkedList<Point>>();
            var visited = new HashSet<Point>();
            var queue = new Queue<Point>();
            var track = new Dictionary<Point, Point>();
            track[start] = start;
            queue.Enqueue(start);
            visited.Add(start);
            while (queue.Count != 0 && chestsRemain != 0)
            {
                var currentPoint = queue.Dequeue();
                foreach (var nextPoint in currentPoint.GetNeighboursOnMap(map))
                {
                    if (visited.Contains(nextPoint)) continue;
                    track[nextPoint] = currentPoint;
                    if (chests.Contains(nextPoint))
                    {
                        var path = GetPath(usedTracks, track, start, nextPoint);
                        yield return path;
                        usedTracks[nextPoint] = path;
                        chestsRemain--;
                    }
                    queue.Enqueue(nextPoint);
                    visited.Add(nextPoint); 
                }
            }
		}
    }
}