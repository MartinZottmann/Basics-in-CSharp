using OpenTK;

namespace MartinZottmann.Engine.Physics
{
    public class Sphere3d
    {
        public Vector3d Origin { get; protected set; }

        public double Radius { get; protected set; }

        public Sphere3d(Vector3d origin, double radius)
        {
            Origin = origin;
            Radius = radius;
        }
    }
}
