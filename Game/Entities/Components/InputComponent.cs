using MartinZottmann.Engine.Entities;
using MartinZottmann.Game.Input;
using System;

namespace MartinZottmann.Game.Entities.Components
{
    [Serializable]
    public class InputComponent : IComponent
    {
        public double Speed = 1.0;

        public InputControlType Type;
    }
}
