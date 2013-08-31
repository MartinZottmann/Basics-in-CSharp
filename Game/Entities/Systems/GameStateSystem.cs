﻿using MartinZottmann.Engine.Entities;
using MartinZottmann.Game.Entities.Nodes;

namespace MartinZottmann.Game.Entities.Systems
{
    public class GameStateSystem : ISystem
    {
        protected NodeList<GameStateNode> game_state_nodes;

        public void Bind(EntityManager entity_manager)
        {
            game_state_nodes = entity_manager.Get<GameStateNode>();
        }

        public void Update(double delta_time)
        {
            foreach (var game_state_node in game_state_nodes)
            {
                // @todo
            }
        }

        public void Render(double delta_time) { }
    }
}
