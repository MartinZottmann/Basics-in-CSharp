using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.States;
using System;

namespace MartinZottmann.Game.Entities.Components
{
    [Serializable]
    public class StateComponent : IComponent
    {
        public IStateMachine<Entity, IComponent> State;
    }
}
