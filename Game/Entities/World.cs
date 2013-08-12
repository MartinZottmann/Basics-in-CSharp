using MartinZottmann.Engine.Physics;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Graphics;
using OpenTK;
using System;
using System.Collections.Generic;

namespace MartinZottmann.Game.Entities
{
    public class World : IDisposable
    {
        public List<GameObject> Children = new List<GameObject>();

        public virtual void Dispose()
        {
            foreach (var child in Children)
                child.Dispose();
        }

        public void AddChild(GameObject child)
        {
            Children.Add(child);
        }

        public void RemoveChild(GameObject child)
        {
            Children.Remove(child);
        }

        public virtual SortedSet<Collision> Intersect(ref Ray3d ray)
        {
            Matrix4d world_model = Matrix4d.Identity;
            var hits = new SortedSet<Collision>();

            foreach (var child in Children)
                if (child.HasComponent<Physic>())
                    foreach (var hit in child.GetComponent<Physic>().Intersect(ref ray, ref world_model))
                        hits.Add(hit);

            return hits;
        }

        public void Update(double delta_time, RenderContext render_context)
        {
            var collisions = DetectCollisions();

            foreach (var child in Children)
                if (child.HasComponent<Physic>())
                    child.GetComponent<Physic>().UpdateVelocity(delta_time);

            ApplyImpusles(collisions, delta_time);

            foreach (var child in Children)
                if (child.HasComponent<Physic>())
                    child.GetComponent<Physic>().UpdatePosition(delta_time);

            Children.ForEach(s => s.Update(delta_time, render_context));
        }

        public void Render(double delta_time, RenderContext render_context)
        {
            foreach (var child in Children)
                child.Render(delta_time, render_context);
        }

        protected List<Collision> DetectCollisions()
        {
            var collisions = new List<Collision>();

            foreach (GameObject e0 in Children)
            {
                if (!e0.HasComponent<Physic>())
                    continue;

                foreach (GameObject e1 in Children)
                {
                    if (e0 == e1)
                        continue;

                    if (!e1.HasComponent<Physic>())
                        continue;

                    var o0 = e0;
                    var o1 = e1;

                    if (!o0.GetComponent<Physic>().BoundingBox.Intersect(ref o0.Position, ref o1.GetComponent<Physic>().BoundingBox, ref o1.Position))
                        continue;

                    var collision = o0.GetComponent<Physic>().BoundingSphere.At(ref o0.Position).Collides(o1.GetComponent<Physic>().BoundingSphere.At(ref o1.Position));
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
                ((GameObject)collision.Object0).GetComponent<Physic>().OnCollision(collision);
        }
    }
}
