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
    }
}
