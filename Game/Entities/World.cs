using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Physics;
using MartinZottmann.Engine.Resources;
using OpenTK;
using System.Collections.Generic;

namespace MartinZottmann.Game.Entities
{
    public class World : Entity
    {
        public World(ResourceManager resources) : base(resources) { }

        public virtual SortedSet<Collision> Intersect(ref Ray3d ray)
        {
            Matrix4d world_model = Matrix4d.Identity;
            var hits = new SortedSet<Collision>();

            foreach (var child in children)
                if (child is Physical)
                    foreach (var hit in (child as Physical).Intersect(ref ray, ref world_model))
                        hits.Add(hit);

            return hits;
        }

        public override void Update(double delta_time, RenderContext render_context)
        {
            children.RemoveAll(s => s.Destroyed);

            var collisions = DetectCollisions();

            foreach (var child in children)
                if (child is Physical)
                    (child as Physical).UpdateVelocity(delta_time);

            ApplyImpusles(collisions, delta_time);

            foreach (var child in children)
                if (child is Physical)
                    (child as Physical).UpdatePosition(delta_time);

            //children.ForEach(s => s.Update(delta_time, render_context));
            foreach (var child in children)
            {
                child.Update(delta_time, render_context);

                child.Reposition(100, 100, 100);
            }
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            foreach (var child in children)
                child.Render(delta_time, render_context);
        }

        protected List<Collision> DetectCollisions()
        {
            var collisions = new List<Collision>();

            foreach (Entities.Entity e0 in children)
            {
                if (!(e0 is Physical))
                    continue;

                foreach (Entities.Entity e1 in children)
                {
                    if (e0 == e1)
                        continue;

                    if (!(e1 is Physical))
                        continue;

                    var o0 = (Physical)e0;
                    var o1 = (Physical)e1;

                    if (!o0.BoundingBox.Intersect(ref o0.Position, ref o1.BoundingBox, ref o1.Position))
                        continue;

                    var collision = o0.BoundingSphere.At(ref o0.Position).Collides(o1.BoundingSphere.At(ref o1.Position));
                    if (collision == null)
                        continue;

                    collisions.Add(
                        new Collision()
                        {
                            HitPoint = collision.HitPoint,
                            Normal = collision.Normal,
                            Object0 = o0,
                            Object1 = o1,
                            Parent = this,
                            PenetrationDepth = collision.PenetrationDepth
                        }
                    );
                }
            }

            return collisions;
        }

        protected void ApplyImpusles(List<Collision> collisions, double delta_time)
        {
            foreach (var collision in collisions)
                (collision.Object0 as Physical).OnCollision(collision);
        }
    }
}
