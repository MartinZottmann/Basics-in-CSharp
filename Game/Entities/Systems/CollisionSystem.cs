using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Physics;
using MartinZottmann.Game.Entities.Nodes;
using System.Collections.Generic;

namespace MartinZottmann.Game.Entities.Systems
{
    public class CollisionSystem : ISystem
    {
        protected NodeList<CollisionNode> collision_nodes;

        public void Bind(EntityManager entitiy_manager)
        {
            collision_nodes = entitiy_manager.Get<CollisionNode>();
        }

        public void Update(double delta_time)
        {
            var collisions = DetectCollisions();

            ApplyImpusles(collisions);
        }

        public void Render(double delta_time) { }

        protected List<Collision> DetectCollisions()
        {
            var collisions = new List<Collision>();

            foreach (var e0 in collision_nodes)
            {
                foreach (var e1 in collision_nodes)
                {
                    if (e0 == e1)
                        continue;

                    if ((e0.Collision.Group & e1.Collision.Group) == CollisionGroups.None)
                        continue;

                    //if (e0.Base.Parent != e1.Base.Parent)
                    //    continue;

                    //if (!e0.Physic.BoundingBox.Intersect(ref e0.Base.Position, ref e1.Physic.BoundingBox, ref e1.Base.Position))
                    //    continue;

                    var collision = e0.Physic.BoundingSphere.At(ref e0.Base.Position).Collides(e1.Physic.BoundingSphere.At(ref e1.Base.Position));
                    if (null == collision)
                        continue;

                    collisions.Add(
                        new Collision()
                        {
                            HitPoint = collision.HitPoint,
                            Normal = collision.Normal,
                            Object0 = e0,
                            Object1 = e1,
                            PenetrationDepth = collision.PenetrationDepth
                        }
                    );
                }
            }

            return collisions;
        }

        protected void ApplyImpusles(List<Collision> collisions)
        {
            foreach (var collision in collisions)
                ((CollisionNode)collision.Object0).OnCollision(collision);
        }
    }
}
