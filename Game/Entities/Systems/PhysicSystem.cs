using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Entities.Nodes;

namespace MartinZottmann.Game.Entities.Systems
{
    public class PhysicSystem : ISystem
    {
        public EntityManager EntityManager;

        protected NodeList<PhysicNode> physic_nodes;

        public void Bind(EntityManager entitiy_manager)
        {
            EntityManager = entitiy_manager;
            physic_nodes = EntityManager.Get<PhysicNode>();
        }

        public void Update(double delta_time)
        {
            foreach (var physic_node in physic_nodes)
            {
                if (physic_node.Base.ParentName != null && physic_node.Base.parent_base == null)
                    physic_node.Base.parent_base = EntityManager.Get(physic_node.Base.ParentName).Get<BaseComponent>();

                physic_node.UpdateVelocity(delta_time);
                physic_node.UpdatePosition(delta_time);
            }
        }

        public void Render(double delta_time) { }
    }
}
