using System;

namespace MartinZottmann.Game.Entities
{
    [Flags]
    [Serializable]
    public enum CollisionGroups
    {
        None = 0,
        Space = 1,
        All = 0xFF
    }
}
