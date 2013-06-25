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
            return discrim >= 0;
        }

        public bool Intersect(ref Ray3d r, Vector3d position, out double distance_near, out double distance_far)
        {
            Vector3d hit_near;
            Vector3d hit_far;
            if (Intersect(ref r, position, out hit_near, out hit_far))
            {
                distance_near = hit_near.Length;
                distance_far = hit_far.Length;
                return true;
            }
            else
            {
                distance_near = System.Double.MaxValue;
                distance_far = System.Double.MinValue;
                return false;
            }
        }

        public bool Intersect(ref Ray3d r, Vector3d position, out Vector3d hit_near, out Vector3d hit_far)
        {
            var A = Vector3d.Dot(r.Direction, r.Direction);
            var p = r.Origin - (Origin + position);
            var B = Vector3d.Dot(r.Direction, p) * 2;
            var C = Vector3d.Dot(p, p) - Radius * Radius;
            var discrim = B * B - 4 * (A * C);
            if (discrim >= 0)
            {
                var disc_root = System.Math.Sqrt(discrim);
                hit_near = r.Direction * (-B - disc_root) / 2 * A;
                hit_far = r.Direction * (-B + disc_root) / 2 * A;
                return true;
            }
            else
            {
                hit_near = Vector3d.Zero;
                hit_far = Vector3d.Zero;
                return false;
            }
        }
    }
}
