using OpenTK;

namespace MartinZottmann.Engine.Physics
{
    public class Ray3d
    {
        public Vector3d Origin { get; protected set; }

        public Vector3d Direction { get; protected set; }

        public Vector3d DirectionFraction { get; protected set; }

        public bool[] Sign { get; protected set; }

        public Ray3d(Vector3d origin, Vector3d direction)
        {
            Origin = origin;
            Direction = direction;
            DirectionFraction = new Vector3d(
                1.0 / Direction.X,
                1.0 / Direction.Y,
                1.0 / Direction.Z
            );
            Sign = new bool[] {
                DirectionFraction.X < 0,
                DirectionFraction.Y < 0,
                DirectionFraction.Z < 0
            };
        }

        public bool Intersect(Plane3d plane, out Vector3d contact)
        {
            double d;
            var r = Intersect(plane, out d);
            contact = Origin + Direction * d;
            return r;
        }

        public bool Intersect(Plane3d plane, out double d)
        {
            d = Vector3d.Dot(plane.Origin - Origin, plane.Normal) / Vector3d.Dot(plane.Normal, Direction);
            return d >= 0;
        }
    }
}
