using System;
using NUnit.Framework;

namespace Manipulation
{
    public static class ManipulatorTask
    {
        public static double[] MoveManipulatorTo(double x, double y, double alpha)
        {
            double tempX = x + Manipulator.Palm * Math.Cos(Math.PI - alpha);
            double tempY = y + Manipulator.Palm * Math.Sin(Math.PI - alpha);
            double elbow = TriangleTask.GetABAngle(Manipulator.Forearm,
                                                Manipulator.UpperArm,
                                                Math.Sqrt(tempX * tempX + tempY * tempY));
            double temp1 = Math.Atan2(tempY, tempX);
            double temp2 = TriangleTask.GetABAngle(Manipulator.UpperArm,
                                                Math.Sqrt(tempX * tempX + tempY * tempY),
                                                Manipulator.Forearm);
            double shoulder = temp1 + temp2;
            double wrist = 2 * Math.PI - alpha - elbow - shoulder;
            if (double.IsNaN(shoulder) || double.IsNaN(elbow) || double.IsNaN(wrist))
                return new[] { double.NaN, double.NaN, double.NaN };
            return new[] { shoulder, elbow, wrist };
        }
    }

    [TestFixture]
    public class ManipulatorTask_Tests
    {
        [Test]
        public void TestMoveManipulatorTo()
        {
            Random random = new Random();
            double x = random.NextDouble() * 10;
            double y = random.NextDouble() * 10;
            double alpha = random.NextDouble() * Math.PI;
            var actualResult = ManipulatorTask.MoveManipulatorTo(x, y, alpha);
            double wristX = x - Manipulator.Palm * Math.Cos(alpha);
            double wristY = y + Manipulator.Palm * Math.Sin(alpha);
            double distance = Math.Sqrt(wristX * wristX + wristY * wristY);
            if (distance > Manipulator.UpperArm + Manipulator.Forearm)
                Assert.AreEqual(new[] { double.NaN, double.NaN, double.NaN }, actualResult);
            else
                Assert.AreNotEqual(new[] { double.NaN, double.NaN, double.NaN }, actualResult);
        }
    }
}