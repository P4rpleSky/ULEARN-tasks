using System;

namespace func_rocket
{
	public class ControlTask
	{
        public static Turn ControlRocket(Rocket rocket, Vector target)
        {
            var distance = new Vector(target.X - rocket.Location.X, target.Y - rocket.Location.Y);
            double totalAngle;
            if (Math.Abs(distance.Angle - rocket.Direction) < 0.5
                || Math.Abs(distance.Angle - rocket.Velocity.Angle) < 0.5)
            {
                totalAngle = (distance.Angle - rocket.Direction + distance.Angle - rocket.Velocity.Angle) / 2;
            }
            else if (Math.Abs(distance.Angle - rocket.Direction) >= 0.5)
            {
                totalAngle = distance.Angle - rocket.Direction;
            }
            else
            {
                totalAngle = distance.Angle - rocket.Velocity.Angle;
            }
            return totalAngle > 0 ? Turn.Right : totalAngle < 0 ? Turn.Left : Turn.None;
        }
    }
}