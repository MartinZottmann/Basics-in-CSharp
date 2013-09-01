using System;

namespace MartinZottmann.Game.Input
{
    [Serializable]
    public enum InputControlType
    {
        None = 0,
        Direct = 1,
        Force = 2,
        Impulse = 3,
        Velocity = 4
    }
}
