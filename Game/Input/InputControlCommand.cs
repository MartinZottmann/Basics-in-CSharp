using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartinZottmann.Game.Input
{
    [Serializable]
    public enum InputControlCommand
    {
        None,
        Forward,
        Backward,
        StrafeLeft,
        StrafeRight,
        StrafeUp,
        StrafeDown,
        TurnLeft,
        TurnRight,
        TurnUp,
        TurnDown,
        RollLeft,
        RollRight
    }
}
