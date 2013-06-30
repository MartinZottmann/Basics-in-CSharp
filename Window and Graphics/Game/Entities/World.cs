using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Resources;
using OpenTK;
using System.Collections.Generic;

namespace MartinZottmann.Game.Entities
{
    class World : Entity
    {
        public World(ResourceManager resources) : base(resources) { }

        public override void Update(double delta_time, RenderContext render_context)
        {
            children.RemoveAll(s => s.Destroyed);

            //children.ForEach(s => s.Update(delta_time, render_context));
            foreach (var child in children)
            {
                child.Update(delta_time, render_context);

                child.Reposition(100, 100, 100);
            }

            var collisions = DetectCollisions();

            foreach (var child in children)
                if (child is Physical)
                    (child as Physical).UpdateVelocity(delta_time);

            ApplyImpusles(collisions, delta_time);

            foreach (var child in children)
                if (child is Physical)
                    (child as Physical).UpdatePosition(delta_time);
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            foreach (var child in children)
                child.Render(delta_time, render_context);
        }

        protected List<Collision> DetectCollisions()
        {
            var collisions = new List<Collision>();

            foreach (Entities.Entity a in children)
            {
                if (!(a is Physical))
                    continue;

                foreach (Entities.Entity b in children)
                {
                    if (a == b)
                        continue;

                    if (!(b is Physical))
                        continue;

                    Vector3d hit_a;
                    Vector3d hit_b;
                    double penetration_depth;

                    var i = a as Physical;
                    var j = b as Physical;

                    if (!i.BoundingBox.Intersect(ref i.Position, ref j.BoundingBox, ref j.Position))
                        continue;

                    var s = i.BoundingSphere;
                    s.Origin += i.Position;
                    var t = j.BoundingSphere;
                    t.Origin += j.Position;

                    if (s.Intersect(ref t, out hit_a, out hit_b, out penetration_depth))
                        collisions.Add(
                            new Collision()
                            {
                                HitPoint = i.Position + hit_a,
                                Normal = (i.Position - j.Position).Normalized() * penetration_depth,
                                Object0 = i,
                                Object1 = j,
                                Parent = this,
                                PenetrationDepth = penetration_depth
                            }
                        );
                }
            }

            return collisions;
        }

        protected void ApplyImpusles(List<Collision> collisions, double delta_time)
        {
            foreach (var collision in collisions)
                collision.Object0.OnCollision(collision);
        }
    }

    public struct Collision
    {
        public Vector3d HitPoint;

        public Vector3d Normal;

        public Physical Object0;

        public Physical Object1;

        public Entity Parent;

        public double PenetrationDepth;
    }
}
