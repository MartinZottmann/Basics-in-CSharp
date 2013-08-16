using MartinZottmann.Engine.Physics;
using MartinZottmann.Game.Graphics;
using System.Collections.Generic;

namespace MartinZottmann.Game.Entities.Components
{
    public class ChildrenPhysic : Abstract
    {
        protected Children children;

        public ChildrenPhysic(GameObject game_object) : base(game_object) {
            children = GameObject.GetComponent<Children>();
        }

        public override void Update(double delta_time, RenderContext render_context)
        {
            var collisions = DetectCollisions();

            foreach (var child in children)
                if (child.HasComponent<Physic>())
                    child.GetComponent<Physic>().UpdateVelocity(delta_time);

            ApplyImpusles(collisions, delta_time);

            foreach (var child in children)
                if (child.HasComponent<Physic>())
                    child.GetComponent<Physic>().UpdatePosition(delta_time);
        }

        protected List<Collision> DetectCollisions()
        {
            var collisions = new List<Collision>();

            foreach (GameObject e0 in children)
            {
                if (!e0.HasComponent<Physic>())
                    continue;

                foreach (GameObject e1 in children)
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
