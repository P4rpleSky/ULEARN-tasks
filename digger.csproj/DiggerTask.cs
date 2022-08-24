using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digger
{
    class Terrain : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public string GetImageFileName()
        {
            return "Terrain.png";
        }
    }

    class Player : ICreature
    {
        public static int xPos = -1;
        public static int yPos = -1;
        public CreatureCommand Act(int x, int y)
        {
            if (Game.KeyPressed == Keys.Up && y - 1 > -1 && y - 1 < Game.MapHeight
                && (Game.Map[x, y - 1] is null || Game.Map[x, y - 1].ToString() != "Digger.Sack"))
            {
                xPos = x;
                yPos = y - 1;
                return new CreatureCommand
                {
                    DeltaX = 0,
                    DeltaY = -1
                };
            }
            if (Game.KeyPressed == Keys.Down && y + 1 > -1 && y + 1 < Game.MapHeight
                && (Game.Map[x, y + 1] is null || Game.Map[x, y + 1].ToString() != "Digger.Sack"))
            {
                xPos = x;
                yPos = y + 1;
                return new CreatureCommand
                {
                    DeltaX = 0,
                    DeltaY = 1
                };
            }
            if (Game.KeyPressed == Keys.Left && x - 1 > -1 && x - 1 < Game.MapWidth
                && (Game.Map[x - 1, y] is null || Game.Map[x - 1, y].ToString() != "Digger.Sack"))
            {
                xPos = x - 1;
                yPos = y;
                return new CreatureCommand
                {
                    DeltaX = -1,
                    DeltaY = 0
                };
            }
            if (Game.KeyPressed == Keys.Right && x + 1 > -1 && x + 1 < Game.MapWidth
                && (Game.Map[x + 1, y] is null || Game.Map[x + 1, y].ToString() != "Digger.Sack"))
            {
                xPos = x + 1;
                yPos = y;
                return new CreatureCommand
                {
                    DeltaX = 1,
                    DeltaY = 0
                };
            }
            xPos = x;
            yPos = y;
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject.ToString() == "Digger.Sack" ||
                conflictedObject.ToString() == "Digger.Monster")
                return true;
            return false;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Digger.png";
        }
    }

    class Sack : ICreature
    {
        public int heightOfFall = 0;

        public CreatureCommand Act(int x, int y)
        {
            if (y + 1 < Game.MapHeight && Game.Map[x, y + 1] is null)
            {
                heightOfFall++;
                return new CreatureCommand { DeltaX = 0, DeltaY = 1 };
            }
            if (heightOfFall > 0 && y + 1 < Game.MapHeight &&
                (Game.Map[x, y + 1].ToString() == "Digger.Player" ||
                 Game.Map[x, y + 1].ToString() == "Digger.Monster"))
            {
                heightOfFall++;
                return new CreatureCommand { DeltaX = 0, DeltaY = 1 };
            }
            else if (heightOfFall > 1 && (y + 1 == Game.MapHeight ||
                                     Game.Map[x, y + 1].ToString() == "Digger.Sack" ||
                                     Game.Map[x, y + 1].ToString() == "Digger.Gold" ||
                                     Game.Map[x, y + 1].ToString() == "Digger.Terrain"))
            {
                heightOfFall = 0;
                return new CreatureCommand
                {
                    DeltaX = 0,
                    DeltaY = 0,
                    TransformTo = new Gold()
                };
            }
            heightOfFall = 0;
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Sack.png";
        }
    }

    class Gold : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject.ToString() == "Digger.Player")
            {
                Game.Scores += 10;
                return true;
            }
            if (conflictedObject.ToString() == "Digger.Monster")
                return true;
            return false;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Gold.png";
        }
    }

    class Monster : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            int yTo = 0;
            int xTo = 0;
            if (IfPlayerExists())
            {
                if (Player.xPos == x)
                {
                    if (Player.yPos < y) yTo = -1;
                    else if (Player.yPos > y) yTo = 1;
                }
                else if (Player.yPos == y)
                {
                    if (Player.xPos < x) xTo = -1;
                    else if (Player.xPos > x) xTo = 1;
                }
                else
                {
                    if (Player.xPos < x) xTo = -1;
                    else if (Player.xPos > x) xTo = 1;
                }
            }
            else
                return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };

            if (!(x + xTo >= 0 && x + xTo < Game.MapWidth && y + yTo >= 0 && y + yTo < Game.MapHeight))
                return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };

            var map = Game.Map[x + xTo, y + yTo];
            if (map != null && (map.ToString() == "Digger.Terrain" || map.ToString() == "Digger.Sack" || map.ToString() == "Digger.Monster"))
                return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };

            return new CreatureCommand() { DeltaX = xTo, DeltaY = yTo };
        }

        static private bool IfPlayerExists()
        {
            for (int i = 0; i < Game.MapWidth; i++)
                for (int j = 0; j < Game.MapHeight; j++)
                {
                    if (Game.Map[i, j] != null && Game.Map[i, j].ToString() == "Digger.Player")
                    {
                        Player.xPos = i;
                        Player.yPos = j;
                        return true;
                    }
                }
            return false;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject.ToString() == "Digger.Sack" ||
                conflictedObject.ToString() == "Digger.Monster")
                return true;
            return false;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Monster.png";
        }
    }
}