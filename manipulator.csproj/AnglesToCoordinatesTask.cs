using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            var elbowPos = new PointF();
            elbowPos.X = Manipulator.UpperArm * (float)Math.Cos(shoulder);
            elbowPos.Y = Manipulator.UpperArm * (float)Math.Sin(shoulder);
            var wristPos = new PointF();
            wristPos.X = elbowPos.X + (Manipulator.Forearm *
                  (float)Math.Cos(elbow + shoulder - Math.PI));
            wristPos.Y = elbowPos.Y + (Manipulator.Forearm *
                  (float)Math.Sin(elbow + shoulder - Math.PI));
            var palmEndPos = new PointF();
            palmEndPos.X = wristPos.X + (Manipulator.Palm *
                  (float)Math.Sin(- 3 * Math.PI / 2 + wrist + elbow  + shoulder));
            palmEndPos.Y = wristPos.Y - (Manipulator.Palm *
                  (float)Math.Cos(- 3 * Math.PI / 2 + wrist + elbow + shoulder));
            return new PointF[]
            {
                elbowPos,
                wristPos,
                palmEndPos
            };
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(0, Math.PI, Math.PI, Manipulator.Forearm + Manipulator.Palm + Manipulator.UpperArm, 0)]
        [TestCase(0, 0, 0, Manipulator.UpperArm - Manipulator.Forearm + Manipulator.Palm, 0)]
        [TestCase(Math.PI / 2, Math.PI, Math.PI, 0, Manipulator.Forearm + Manipulator.Palm + Manipulator.UpperArm)]
        [TestCase(Math.PI / 2, 3*Math.PI/2, Math.PI,  - Manipulator.Palm - Manipulator.Forearm, Manipulator.UpperArm)]
        [TestCase(- Math.PI / 2, Math.PI / 2, - Math.PI / 2, -120.0f, -210.0f)]
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");

            Assert.AreEqual(Manipulator.UpperArm, 
                Math.Sqrt(joints[0].X * joints[0].X + joints[0].Y * joints[0].Y), 1e-5, "Upper Arm Lenght");

            Assert.AreEqual(Manipulator.Forearm,
                Math.Sqrt(Math.Pow((joints[1].X - joints[0].X), 2) 
                + Math.Pow((joints[1].Y - joints[0].Y), 2)), 1e-5, "Forearm Lenght");

            Assert.AreEqual(Manipulator.Palm,
                Math.Sqrt(Math.Pow((joints[2].X - joints[1].X), 2)
                + Math.Pow((joints[2].Y - joints[1].Y), 2)), 1e-5, "Palm Lenght");
        }
    }
}