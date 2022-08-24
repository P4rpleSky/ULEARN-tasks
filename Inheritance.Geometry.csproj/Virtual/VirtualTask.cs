using System;
using System.Collections.Generic;
using System.Linq;

namespace Inheritance.Geometry.Virtual
{
    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }

        public abstract bool ContainsPoint(Vector3 point);

        public abstract RectangularCuboid GetBoundingBox();
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var vector = point - Position;
            var length2 = vector.GetLength2();
            return length2 <= this.Radius * this.Radius;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            return new RectangularCuboid(Position, Radius * 2, Radius * 2, Radius * 2);
        }
    }

    public class RectangularCuboid : Body
    {
        public double SizeX { get; }
        public double SizeY { get; }
        public double SizeZ { get; }

        public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var minPoint = new Vector3(
                Position.X - this.SizeX / 2,
                Position.Y - this.SizeY / 2,
                Position.Z - this.SizeZ / 2);
            var maxPoint = new Vector3(
                Position.X + this.SizeX / 2,
                Position.Y + this.SizeY / 2,
                Position.Z + this.SizeZ / 2);
            return point >= minPoint && point <= maxPoint;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            return new RectangularCuboid(Position, SizeX, SizeY, SizeZ);
        }
    }

    public class Cylinder : Body
    {
        public double SizeZ { get; }

        public double Radius { get; }

        public Cylinder(Vector3 position, double sizeZ, double radius) : base(position)
        {
            SizeZ = sizeZ;
            Radius = radius;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var vectorX = point.X - Position.X;
            var vectorY = point.Y - Position.Y;
            var length2 = vectorX * vectorX + vectorY * vectorY;
            var minZ = Position.Z - this.SizeZ / 2;
            var maxZ = minZ + this.SizeZ;
            return length2 <= this.Radius * this.Radius && point.Z >= minZ && point.Z <= maxZ;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            return new RectangularCuboid(Position, 2 * Radius, 2 * Radius, SizeZ);
        }
    }

    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            return this.Parts.Any(body => body.ContainsPoint(point));
        }

        public override RectangularCuboid GetBoundingBox()
        {
            var boundingBoxes = Parts.Select(p => p.GetBoundingBox());
            var maxX = boundingBoxes.Max(p => p.Position.X + p.SizeX / 2);
            var maxY = boundingBoxes.Max(p => p.Position.Y + p.SizeY / 2);
            var maxZ = boundingBoxes.Max(p => p.Position.Z + p.SizeZ / 2);
            var minX = boundingBoxes.Min(p => p.Position.X - p.SizeX / 2);
            var minY = boundingBoxes.Min(p => p.Position.Y - p.SizeY / 2);
            var minZ = boundingBoxes.Min(p => p.Position.Z - p.SizeZ / 2);
            return new RectangularCuboid(
                new Vector3((minX + maxX) / 2, (minY + maxY) / 2, (minZ + maxZ) / 2),
                maxX - minX,
                maxY - minY, 
                maxZ - minZ);
        }
    }
}}