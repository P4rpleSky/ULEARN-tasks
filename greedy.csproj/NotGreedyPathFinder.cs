using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy
{
    public static class DijkstraPathFinderExtensions
    {
        public static IEnumerable<PathWithCost> GetStartPaths
            (this DijkstraPathFinder pathFinder, State state, Point position, List<Point> chests, int energyRemain)
            => pathFinder.GetPathsByDijkstra(state, position, chests)
                .Where(x => x.Cost <= energyRemain)
                .Where(x => !x.Path.Any(point => chests.Except(x.Endpoints).Contains(point)));
    }

    public class NotGreedyPathFinder : IPathFinder
    {
        private class LinkedPath
        {
            public LinkedPath Previous { get; set; }
            public PathWithCost PathWithCost { get; set; }
            public int Energy { get; private set; }
            public int Score { get; private set; }
            public HashSet<Point> VisitedChests { get; private set; }

            public LinkedPath(LinkedPath previous, PathWithCost currentPath)
            {
                Previous = previous;
                PathWithCost = currentPath;
                Score = previous is null ? 1 : Previous.Score + 1;
                Energy = previous is null ? currentPath.Cost : Previous.Energy + currentPath.Cost;
                VisitedChests = previous is null ? new HashSet<Point>() : new HashSet<Point>(previous.VisitedChests);
                VisitedChests.Add(currentPath.End);
            }
        }

        private static IEnumerable<PathWithCost> GetPathsToUnvisitedChests
            (DijkstraPathFinder pathFinder, State state, LinkedPath startPath,
            Point position, List<Point> chests, int energyRemain)
        {
            var unvisitedChests = chests.Where(x => !startPath.VisitedChests.Contains(x));
            return pathFinder.GetPathsByDijkstra(state, position, chests)
                .Where(x => x.Cost <= energyRemain)
                .Where(pathWithCost => !startPath.VisitedChests.Contains(pathWithCost.End))
                .Where(x => !x.Path.Any(point => unvisitedChests.Except(x.Endpoints).Contains(point)));
        }

        public List<Point> FindPathToCompleteGoal(State state)
        {
            var pathFinder = new DijkstraPathFinder();
            var allPaths = new HashSet<LinkedPath>();
            var pathsStack = new Stack<LinkedPath>();
            var chests = new List<Point>(state.Chests);
            foreach (var path in pathFinder.GetStartPaths(state, state.Position, chests, state.InitialEnergy))
            {
                var startPath = new LinkedPath(null, path);
                allPaths.Add(startPath);
                pathsStack.Push(startPath);
            }
            while (true)
            {
                if (pathsStack.Count == 0) break;
                var startPath = pathsStack.Pop();
                int energyRemain = state.InitialEnergy - startPath.Energy;
                foreach (var currentPath in
                    GetPathsToUnvisitedChests(pathFinder, state, startPath, startPath.PathWithCost.End, chests, energyRemain))
                {
                    var nextPath = new LinkedPath(startPath, currentPath);
                    pathsStack.Push(nextPath);
                    if (nextPath.Score == chests.Count)
                    {
                        allPaths.Clear();
                        allPaths.Add(nextPath);
                        return GetResultPath(allPaths);
                    }
                    allPaths.Add(nextPath);
                }
            }
            return GetResultPath(allPaths);
        }

        private List<Point> GetResultPath(HashSet<LinkedPath> allPaths)
        {
            var result = new List<Point>();
            var linkedPath = allPaths.OrderByDescending(x => x.Score)
                .ThenByDescending(x => x.Energy)
                .FirstOrDefault();
            var tempWay = linkedPath.PathWithCost.Path.Skip(1).Reverse();
            result.AddRange(tempWay);
            while (!(linkedPath.Previous is null))
            {
                tempWay = linkedPath.Previous.PathWithCost.Path.Skip(1).Reverse();
                result.AddRange(tempWay);
                linkedPath = linkedPath.Previous;
            }
            result.Reverse();
            return result;
        }
    }
}