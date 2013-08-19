using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Physics;
using MartinZottmann.Game.Entities.Nodes;
using System.Collections.Generic;

namespace MartinZottmann.Game.Entities.Systems
{
    public class PhysicSystem : ISystem
    {
        protected NodeList<PhysicNode> physic_nodes;

        public void Bind(EntityManager entitiy_manager)
        {
            physic_nodes = entitiy_manager.Get<PhysicNode>();
        }

        public void Update(double delta_time)
        {
            var collisions = DetectCollisions();

            foreach (var physic_node in physic_nodes)
                physic_node.UpdateVelocity(delta_time);

            ApplyImpusles(collisions, delta_time);

            foreach (var physic_node in physic_nodes)
                physic_node.UpdatePosition(delta_time);
        }

        public void Render(double delta_time)
        {
            // void
        }

        protected List<Collision> DetectCollisions()
        {
            var collisions = new List<Collision>();

            foreach (var e0 in physic_nodes)
            {
                foreach (var e1 in physic_nodes)
                {
                    if (e0 == e1)
                        continue;

                    var p0 = e0.Physic;
                    var p1 = e1.Physic;
                    var b0 = e0.Base;
                    var b1 = e1.Base;

                    if (!p0.BoundingBox.Intersect(ref b0.Position, ref p1.BoundingBox, ref b1.Position))
                        continue;

                    var collision = p0.BoundingSphere.At(ref b0.Position).Collides(p1.BoundingSphere.At(ref b1.Position));
                    if (collision == null)
                        continue;

                    collisions.Add(
                        new Collision()
                        {
                            HitPoint = collision.HitPoint,
                            Normal = collision.Normal,
                            Object0 = e0,
                            Object1 = e1,
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
                ((PhysicNode)collision.Object0).OnCollision(collision);
        }
    }
}
