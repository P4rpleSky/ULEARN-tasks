using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{	
	public class DungeonTask
	{
        private static IEnumerable<SinglyLinkedList<Point>> FindWaysFromStartToChests(Map map)
            => BfsTask.FindPaths(map, map.InitialPosition, map.Chests);

        private static IEnumerable<SinglyLinkedList<Point>> FindWaysFromExitToChests(Map map)
            => BfsTask.FindPaths(map, map.Exit, map.Chests);

        private static IEnumerable<Point> FindWayFromStartToExit(Map map)
            => BfsTask.FindPaths(map, map.Exit, new[] { map.InitialPosition }).FirstOrDefault();

        private static MoveDirection[] ConvertToMoveDirection(IEnumerable<Point> way)
           => way is null? new MoveDirection[0] :
                    way.Zip(way.Skip(1), (p1, p2) =>
                        Walker.ConvertOffsetToDirection(new Size(p2.X - p1.X, p2.Y - p1.Y)))
                       .ToArray();


        public static MoveDirection[] FindShortestPath(Map map)
        {
            var shortestWay = (from startWay in FindWaysFromStartToChests(map)
                               join endWay in FindWaysFromExitToChests(map) on startWay.Value equals endWay.Value
                               select Tuple.Create(startWay, endWay.Previous))
                       .OrderByDescending(x => x.Item1.Length + x.Item2.Length)
                       .Select(x => x.Item1.Reverse().Concat(x.Item2))
                       .LastOrDefault();
            shortestWay = shortestWay is null ? FindWayFromStartToExit(map) : shortestWay;
            return ConvertToMoveDirection(shortestWay);
        }
    }
}