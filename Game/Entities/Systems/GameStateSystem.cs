using MartinZottmann.Engine.Entities;
using MartinZottmann.Game.Entities.Nodes;

namespace MartinZottmann.Game.Entities.Systems
{
    public class GameStateSystem : ISystem
    {
        protected EntityManager entity_manager;

        protected NodeList<GameStateNode> game_state_nodes;

        public void Start(EntityManager entity_manager)
        {
            this.entity_manager = entity_manager;

            game_state_nodes = this.entity_manager.GetNodeList<GameStateNode>();
        }

        public void Update(double delta_time)
        {
            foreach (var game_state_node in game_state_nodes)
            {
                if (null == game_state_node.GameState.camera_entity)
                    game_state_node.GameState.camera_entity = entity_manager.GetEntity(game_state_node.GameState.CameraEntityName);
            }
        }

        public void Render(double delta_time) { }

        public void Stop()
        {
            game_state_nodes = null;
        }
    }
}
