using MartinZottmann.Engine.Entities;
using OpenTK;
using System;

namespace MartinZottmann.Game.Entities.Components
{
    [Serializable]
    public class AIComponent : IComponent
    {
        public Vector3d? TargetPosition;
    }
}
