
using System.Collections.Generic;
using System.Linq;

namespace Inheritance.Geometry.Visitor
{
    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }

        public abstract Body Accept(IVisitor visitor);
    }

    public interface IVisitor
    {
        Body Visit(Ball ball);
        Body Visit(RectangularCuboid rectCub);
        Body Visit(Cylinder cyl);
        Body Visit(CompoundBody compBody);
    }

    public class BoundingBoxVisitor : IVisitor
    {
        public Body Visit(Ball ball)
        {
            return new RectangularCuboid(ball.Position, ball.Radius * 2, ball.Radius * 2, ball.Radius * 2);
        }

        public Body Visit(RectangularCuboid rectCub)
        {
            return new RectangularCuboid(rectCub.Position, rectCub.SizeX, rectCub.SizeY, rectCub.SizeZ);
        }

        public Body Visit(Cylinder cyl)
        {
            return new RectangularCuboid(cyl.Position, 2 * cyl.Radius, 2 * cyl.Radius, cyl.SizeZ);
        }

        public Body Visit(CompoundBody compBody)
        {
            var boundingBoxes = compBody.Parts.Select(p => (RectangularCuboid)(p.Accept(new BoundingBoxVisitor())));
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

    public class BoxifyVisitor : IVisitor
    {
        public Body Visit(Ball ball)
        {
            return ball.Accept(new BoundingBoxVisitor());
        }

        public Body Visit(RectangularCuboid rectCub)
        {
            return rectCub;
        }

        public Body Visit(Cylinder cyl)
        {
            return cyl.Accept(new BoundingBoxVisitor());
        }

        public Body Visit(CompoundBody compBody)
        {
            var boxedParts = compBody.Parts.Select(p => p.Accept(new BoxifyVisitor()));
            return new CompoundBody(boxedParts.ToList());
        }
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
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

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
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

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }

        public override Body Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}