using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Entities.Nodes;

namespace MartinZottmann.Game.Entities.Systems
{
    public class CameraSystem : ISystem
    {
        public Camera Camera;

        protected NodeList<GameStateNode> game_state_nodes;

        public CameraSystem(Camera camera)
        {
            Camera = camera;
        }

        public void Start(EntityManager entity_manager)
        {
            game_state_nodes = entity_manager.GetNodeList<GameStateNode>();
        }

        public void Update(double delta_time)
        {
            Camera.Update(delta_time);

            foreach (var game_state_node in game_state_nodes)
            {
                var camera_entity = game_state_node.GameState.camera_entity;
                var camera_entity_base = camera_entity.Get<BaseComponent>();

                Camera.Position = camera_entity_base.WorldPosition;
                camera_entity_base.Orientation = Camera.Orientation;
            }
        }

        public void Render(double delta_time) { }

        public void Stop()
        {
            game_state_nodes = null;
        }
    }
}
