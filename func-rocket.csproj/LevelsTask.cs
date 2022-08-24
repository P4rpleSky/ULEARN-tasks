using System;
using System.Collections.Generic;
using System.Drawing;

namespace func_rocket
{
    public class LevelsTask
	{
		static readonly Physics stdPhysics = new Physics();
		static readonly Rocket stdRocket = new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);
		static readonly Vector stdTarget = new Vector(600, 200);

		private static Vector GetGravityUp(Size size, Vector location) 
		{
			var distance = Math.Abs(location.Y - size.Height);
			var gravity = - 300 / (distance + 300);
			return new Vector(0, gravity);
		}

		private static Vector GetGravityWhiteHole(Vector location, Vector target)
		{
			var distance = Math.Sqrt(Math.Pow(location.X - target.X, 2) + Math.Pow(location.Y - target.Y, 2));
			var module = 140 * distance / (distance * distance + 1);
			return new Vector(location.X - target.X, location.Y - target.Y).Normalize() * module;
		}

		private static Vector GetGravityBlackHole(Vector start, Vector location, Vector target)
		{
			var anomaly = new Vector((start.X + target.X) / 2, (start.Y + target.Y) / 2);
			var distance = Math.Sqrt(Math.Pow(location.X - anomaly.X, 2) + Math.Pow(location.Y - anomaly.Y, 2));
			var module = 300 * distance / (distance * distance + 1);
			return new Vector(anomaly.X - location.X, anomaly.Y - location.Y).Normalize() * module;
		}

		private static Level CreateLevelZero() 
			=> new Level("Zero", stdRocket, stdTarget, (size, v) => Vector.Zero, stdPhysics);

		private static Level CreateLevelHeavy()
			=> new Level("Heavy", stdRocket, stdTarget, (size, v) => new Vector(0, 0.9), stdPhysics);

		private static Level CreateLevelUp() 
			=> new Level("Up",
				new Rocket(new Vector(400, 100), Vector.Zero, 0),
				new Vector(700, 500),
				(size, v) => GetGravityUp(size, v), stdPhysics);

		public static Level CreateLevelWhiteHole() 
			=> new Level("WhiteHole", stdRocket, stdTarget, 
				(size, v) => GetGravityWhiteHole(v, stdTarget), stdPhysics);

		public static Level CreateLevelBlackHole() 
			=> new Level("BlackHole", stdRocket, stdTarget, 
				(size, v) => GetGravityBlackHole(stdRocket.Location, v, stdTarget), stdPhysics);

		public static Level CreateLevelBlackAndWhite() 
			=> new Level("BlackAndWhite", stdRocket, stdTarget,
				(size, v) => 
				(GetGravityWhiteHole(v, stdTarget) + GetGravityBlackHole(stdRocket.Location, v, stdTarget)) / 2, 
				stdPhysics);

		public static IEnumerable<Level> CreateLevels()
		{
			yield return CreateLevelZero();
			yield return CreateLevelHeavy();
			yield return CreateLevelUp();
			yield return CreateLevelWhiteHole();
			yield return CreateLevelBlackHole();
			yield return CreateLevelBlackAndWhite();
		}
	}
}