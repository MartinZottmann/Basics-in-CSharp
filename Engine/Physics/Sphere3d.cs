using OpenTK;
using System;

namespace MartinZottmann.Engine.Physics
{
    public struct Sphere3d : ICollidable, ICollidable<Ray3d>, ICollidable<Sphere3d>
    {
        public Vector3d Origin;

        public double Radius;

        public Sphere3d(Vector3d origin, double radius)
        {
            Origin = origin;
            Radius = radius;
        }

        public Sphere3d At(ref Vector3d position_world)
        {
            var copy = this;
            Vector3d.Add(ref copy.Origin, ref position_world, out copy.Origin);
            return copy;
        }

        public Sphere3d At(ref Matrix4d model_world)
        {
            var copy = this;
            Vector3d.Transform(ref copy.Origin, ref model_world, out copy.Origin);
            return copy;
        }

        public Collision Collides(object @object)
        {
            throw new NotImplementedException();
        }

        public Collision Collides(Ray3d @object)
        {
            return Collides(ref @object);
        }

        public Collision Collides(ref Ray3d @object)
        {
            var direction = @object.Direction.Normalized();
            var A = Vector3d.Dot(direction, direction);
            var p = @object.Origin - Origin;
            var B = Vector3d.Dot(direction, p) * 2;
            var C = Vector3d.Dot(p, p) - Radius * Radius;
            var discrim = B * B - 4 * (A * C);
            if (discrim >= 0)
            {
                var disc_root = Math.Sqrt(discrim);
                var h0 = @object.Origin + direction * (-B - disc_root) / 2 * A;
                return new Collision()
                {
                    HitPoint = h0,
                    //HitPoint1 = @object.Direction * (-B + disc_root) / 2 * A,
                    Normal = (h0 - Origin).Normalized(),
                    Object0 = this,
                    Object1 = @object,
                    PenetrationDepth = 0.0 // @todo
                };
            }

            return null;
        }

        public Collision Collides(Sphere3d @object)
        {
            return Collides(ref @object);
        }

        public Collision Collides(ref Sphere3d @object)
        {
            var relative = Origin - @object.Origin;
            var distance = relative.Length;
            if (distance <= Radius + @object.Radius)
            {
                relative /= distance;
                var penetration_depth = Radius + @object.Radius - distance;
                return new Collision()
                {
                    HitPoint = Origin + - relative * Radius,
                    //HitPoint1 = relative * Radius,
                    Normal = relative,
                    Object0 = this,
                    Object1 = @object,
                    PenetrationDepth = penetration_depth
                };
            }

            return null;
        }
    }
}
