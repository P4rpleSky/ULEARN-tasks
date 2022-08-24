namespace Mazes
{
	public static class EmptyMazeTask
	{
		public static void MoveRightTillEnd(Robot robot, int mazeWidth)
        {
			for(int x = 1; x < mazeWidth; x++)
				robot.MoveTo(Direction.Right);
        }

		public static void MoveDownTillEnd(Robot robot, int mazeHeight)
		{
			for (int y = 1; y < mazeHeight; y++)
				robot.MoveTo(Direction.Down);
		}

		public static void MoveOut(Robot robot, int width, int height)
		{
			int mazeWidth = width - 2;
			int mazeHeight = height - 2;
			MoveRightTillEnd(robot, mazeWidth);
			MoveDownTillEnd(robot, mazeHeight);
		}
	}
}