using MartinZottmann.Engine.Entities;
using System;

namespace MartinZottmann.Game.Entities.Components
{
    [Serializable]
    public class GameStateComponent : IComponent
    {
        public bool Debug = false;
    }
}
