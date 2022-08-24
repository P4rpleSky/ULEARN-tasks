using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.MapObjects
{
    public interface IHaveOwner
    {
        int Owner { get; set; }
    }

    public interface IHaveArmy
    {
        Army Army { get; set; }
    }

    public interface IHaveTreasure
    {
        Treasure Treasure { get; set; }
    }
    
    public class Dwelling : IHaveOwner
    {
        public int Owner { get; set; }
    }

    public class Mine : IHaveOwner, IHaveArmy, IHaveTreasure
    {
        public int Owner { get; set; }
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Creeps : IHaveArmy, IHaveTreasure
    {
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Wolves : IHaveArmy
    {
        public Army Army { get; set; }
    }

    public class ResourcePile : IHaveTreasure
    {
        public Treasure Treasure { get; set; }
    }

    public static class Interaction
    {
        public static void Make(Player player, object mapObject)
        {
            if (mapObject is IHaveArmy objWithArmy && !player.CanBeat(objWithArmy.Army))
                player.Die();
            else
            {
                if (mapObject is IHaveOwner objWithOwner)
                    objWithOwner.Owner = player.Id;
                if (mapObject is IHaveTreasure objWithTreasure)
                    player.Consume(objWithTreasure.Treasure);
            }
        }
    }
}