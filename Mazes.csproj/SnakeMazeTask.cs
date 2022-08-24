namespace Mazes
{
	public static class SnakeMazeTask
	{
		public static void MoveRightTillEnd(Robot robot, int mazeWidth)
		{
			for (int x = 1; x < mazeWidth; x++)
				robot.MoveTo(Direction.Right);
		}

		public static void MoveLeftTillEnd(Robot robot, int mazeWidth)
		{
			for (int x = mazeWidth; x > 1; x--)
				robot.MoveTo(Direction.Left);
		}

		public static void CycleMove (Robot robot, int mazeWidth, int mazeHeight)
        {
			MoveRightTillEnd(robot, mazeWidth);
			robot.MoveTo(Direction.Down);
			robot.MoveTo(Direction.Down);
			MoveLeftTillEnd(robot, mazeWidth);
		}


		public static void MoveOut(Robot robot, int width, int height)
		{
			int mazeWidth = width - 2;
			int mazeHeight = height - 2;
			for (int y = 1; y <= (mazeHeight - 2) / 4; y++)
            {
				CycleMove(robot, mazeWidth, mazeHeight);
				robot.MoveTo(Direction.Down);
				robot.MoveTo(Direction.Down);
			}
			CycleMove(robot, mazeWidth, mazeHeight);
		}
	}
}