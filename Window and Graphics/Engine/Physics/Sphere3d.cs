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

        public bool Intersect(ref Ray3d r, Vector3d position)
        {
            var A = Vector3d.Dot(r.Direction, r.Direction);
            var p = r.Origin - (Origin + position);
            var B = Vector3d.Dot(r.Direction, p) * 2;
            var C = Vector3d.Dot(p, p) - Radius * Radius;
            var discrim = B * B - 4 * (A * C);
            return discrim > 0;
        }
    }
}
