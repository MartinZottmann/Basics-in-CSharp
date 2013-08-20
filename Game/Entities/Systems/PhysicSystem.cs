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
            foreach (var physic_node in physic_nodes)
            {
                physic_node.UpdateVelocity(delta_time);
                physic_node.UpdatePosition(delta_time);
            }
        }

        public void Render(double delta_time) { }
    }
}
