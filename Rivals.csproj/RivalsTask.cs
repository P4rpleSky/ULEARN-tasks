using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rivals
{
	public static class OwnedLocationExtensions
	{
		public static IEnumerable<OwnedLocation> GetNeighboursOnMap(this OwnedLocation place, Map map)
		{
            for (int dx = -1; dx < 2; dx++)
                for (int dy = -1; dy < 2; dy++)
                    if (Math.Abs(dx - dy) == 1 &&
                        map.InBounds(new Point(place.Location.X + dx, place.Location.Y + dy)) &&
                        map.Maze[place.Location.X + dx, place.Location.Y + dy] == MapCell.Empty)
                        yield return new OwnedLocation
                            (
                                place.Owner,
                                new Point(place.Location.X + dx, place.Location.Y + dy),
                                place.Distance + 1
                            );
        }
	}

	public class RivalsTask
	{
		public static IEnumerable<OwnedLocation> AssignOwners(Map map)
		{
            var visited = new HashSet<Point>();
            var queue = new Queue<OwnedLocation>();
            for (int player = 0; player < map.Players.Length; player++)
            {
                queue.Enqueue(new OwnedLocation(player, map.Players[player], 0));
                visited.Add(map.Players[player]);
                yield return new OwnedLocation(player, map.Players[player], 0);
            }
            while (queue.Count != 0)
            {
                var ownedLocation = queue.Dequeue();
                foreach (var nextOwnedLocation in ownedLocation.GetNeighboursOnMap(map))
                {
                    if (visited.Contains(nextOwnedLocation.Location)) continue;
                    yield return nextOwnedLocation;
                    queue.Enqueue(nextOwnedLocation);
                    visited.Add(nextOwnedLocation.Location);
                }
            }
        }
	}
}