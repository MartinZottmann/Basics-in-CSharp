using MartinZottmann.Engine.Entities;
using System;

namespace MartinZottmann.Game.Entities.Components
{
    [Serializable]
    public class CollisionComponent : IComponent
    {
        public CollisionGroups Group = CollisionGroups.All;
    }
}
