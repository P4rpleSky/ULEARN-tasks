using System.Collections.Generic;
using System.Drawing;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;
using System.Linq;

namespace Greedy
{
	public class GreedyPathFinder : IPathFinder
	{
		public List<Point> FindPathToCompleteGoal(State state)
		{
			var result = new List<PathWithCost>();
			var pathFinder = new DijkstraPathFinder();
			var chests = new HashSet<Point>(state.Chests);
			var goal = state.Goal;
			var position = state.Position;
			if (goal > chests.Count) return new List<Point>();
			while (goal != 0)
            {
				var pathToNearestChest = pathFinder.GetPathsByDijkstra(state, position, chests)
					.FirstOrDefault();
				if (pathToNearestChest is null) return new List<Point>();
				state.Energy -= pathToNearestChest.Cost;
				if (state.Energy < 0) return new List<Point>();
				position = pathToNearestChest.End;
				result.Add(pathToNearestChest);
				goal--;
				chests.Remove(position);
            }
			return result.SelectMany(x => x.Path.Skip(1)).ToList();
		}
	}
}