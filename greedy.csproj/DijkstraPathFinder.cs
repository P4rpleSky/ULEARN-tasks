using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Greedy.Architecture;
using System.Drawing;

namespace Greedy
{
	public static class PointExtensions
    {
		public static IEnumerable<Point> GetIncidentCells(this Point currentPoint, State state)
        {
			for (int dx = -1; dx < 2; dx++)
				for (int dy = -1; dy < 2; dy++)
					if (Math.Abs(dx - dy) == 1 &&
						state.InsideMap(new Point(currentPoint.X + dx, currentPoint.Y + dy)) &&
						!state.IsWallAt(new Point(currentPoint.X + dx, currentPoint.Y + dy)))
						yield return new Point(currentPoint.X + dx, currentPoint.Y + dy);
		}
    }
	
	class DijkstraData
	{
		public Point Previous { get; set; }
		public int Price { get; set; }
	}

	public class DijkstraPathFinder
    {
        private static readonly Point nullPoint = new Point(-1, -1);
		private static readonly Dictionary
			<Tuple<State, Point, IEnumerable<Point>>,
			List<PathWithCost>> mazePathsPairs = new Dictionary
				<Tuple<State, Point, IEnumerable<Point>>,
				 List<PathWithCost>>();

		private static PathWithCost GetPathWithCosts(Dictionary<Point, DijkstraData> track, Point end)
        {
            var path = new List<Point>();
			var cost = track[end].Price;
            while (end != nullPoint)
            {
                path.Add(end);
                end = track[end].Previous;
            }
            path.Reverse();
            return new PathWithCost(cost, path.ToArray());
        }

		private static List<Point> GetCellsAtState(State state, Point start)
        {
			var cells = new List<Point>();
			for (int y = 0; y < state.MapHeight; y++)
				for (int x = 0; x < state.MapWidth; x++)
					if (!state.IsWallAt(new Point(x, y)))
						cells.Add(new Point(x, y));
			return cells;
		}

		public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start,
            IEnumerable<Point> targets)
        {
			var currentMaze = Tuple.Create(state, start, targets);
			if (mazePathsPairs.ContainsKey(currentMaze))
            {
				foreach (var path in mazePathsPairs[currentMaze])
					yield return path;
				yield break;
			}
			mazePathsPairs[currentMaze] = new List<PathWithCost>();
			var notVisited = GetCellsAtState(state, start);			
			var targetSet = targets.Where(x => state.InsideMap(x) && !state.IsWallAt(x)).ToHashSet();
			var track = new Dictionary<Point, DijkstraData>();
			track[start] = new DijkstraData { Price = 0, Previous = nullPoint };

			while (true)
			{
				Point toOpen = nullPoint;
				var bestPrice = double.PositiveInfinity;
				foreach (var e in notVisited)
				{
					if (track.ContainsKey(e) && track[e].Price < bestPrice)
					{
						bestPrice = track[e].Price;
						toOpen = e;
					}
				}

				if (toOpen == nullPoint) yield break;
				if (targetSet.Contains(toOpen)) 
				{
					var path = GetPathWithCosts(track, toOpen);
					mazePathsPairs[currentMaze].Add(path);
					yield return path;
					targetSet.Remove(toOpen);
				};

				foreach (var nextNode in toOpen.GetIncidentCells(state))
				{
					var currentPrice = track[toOpen].Price + state.CellCost[nextNode.X, nextNode.Y];
					if (!track.ContainsKey(nextNode) || track[nextNode].Price > currentPrice)
					{
						track[nextNode] = new DijkstraData { Previous = toOpen, Price = currentPrice };
					}
				}
				notVisited.Remove(toOpen);
			}
		}
    }
}
