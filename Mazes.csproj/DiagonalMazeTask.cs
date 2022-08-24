namespace Mazes
{
	public static class DiagonalMazeTask
	{
		public static void MoveRightTillEnd(Robot robot, int mazeWidth, int mazeHeight)
		{
			for (int x = 1; x <= mazeWidth / mazeHeight; x++)
				robot.MoveTo(Direction.Right);
		}

		public static void MoveDownTillEnd(Robot robot, int mazeWidth, int mazeHeight)
		{
			for (int y = 1; y <= mazeHeight / mazeWidth; y++)
				robot.MoveTo(Direction.Down);
		}

		public static void CycleToDown(Robot robot, int mazeWidth, int mazeHeight)
        {
			for (int x = 1; x <= mazeWidth - 1; x++)
            {
				MoveDownTillEnd(robot, mazeWidth, mazeHeight);
				robot.MoveTo(Direction.Right);
			}
			MoveDownTillEnd(robot, mazeWidth, mazeHeight);
		}

		public static void CycleToRight(Robot robot, int mazeWidth, int mazeHeight)
		{
			for (int y = 1; y <= mazeHeight - 1; y++)
			{
				MoveRightTillEnd(robot, mazeWidth, mazeHeight);
				robot.MoveTo(Direction.Down);
			}
			MoveRightTillEnd(robot, mazeWidth, mazeHeight);
		}

		public static void MoveOut(Robot robot, int width, int height)
		{
			int mazeWidth = width - 2;
			int mazeHeight = height - 2;
			if (mazeHeight >= mazeWidth)
				CycleToDown(robot, mazeWidth, mazeHeight);
			else
				CycleToRight(robot, mazeWidth, mazeHeight);
		}
	}
}