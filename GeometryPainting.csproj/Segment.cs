using System;

namespace GeometryTasks
{
    public class Vector
    {
        public double X;
        public double Y;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }
        
        public Vector Add(Vector vector2)
        {
            return Geometry.Add(this, vector2);
        }

        public bool Belongs(Segment segment)
        {
            return Geometry.IsVectorInSegment(this, segment);
        }
    }

    public class Segment
    {
        public Vector Begin = new Vector();
        public Vector End = new Vector();

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public bool Contains(Vector vector)
        {
            return Geometry.IsVectorInSegment(vector, this);
        }
    }

    public class Geometry
    {
        public static double GetLength (Vector vector)
        {
            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }
        
        public static Vector Add(Vector vector1, Vector vector2)
        {
            return new Vector
            {
                X = vector1.X + vector2.X,
                Y = vector1.Y + vector2.Y
            };
        }

        public static double GetLength (Segment segment)
        {
            double xLen = segment.End.X - segment.Begin.X;
            double yLen = segment.End.Y - segment.Begin.Y;
            return Math.Sqrt(xLen * xLen + yLen * yLen);
        }

        public static bool IsVectorInSegment(Vector vector, Segment segment)
        {
            if (segment != null && vector != null)
            {
                Segment ax = new Segment
                {
                    Begin = { X = segment.Begin.X, Y = segment.Begin.Y },
                    End = { X = vector.X, Y = vector.Y }
                };
                Segment xb = new Segment
                {
                    Begin = { X = vector.X, Y = vector.Y },
                    End = { X = segment.End.X, Y = segment.End.Y }
                };
                return GetLength(ax) + GetLength(xb) == GetLength(segment);
            }
            return false;
        }
    }
}