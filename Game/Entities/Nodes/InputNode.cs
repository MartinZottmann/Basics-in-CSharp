using MartinZottmann.Engine.Entities;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Input;
using OpenTK;
using System;

namespace MartinZottmann.Game.Entities.Nodes
{
    public class InputNode : Node
    {
        public InputComponent Input;

        public BaseComponent Base;

        public void Control(double delta_time, InputControlCommand command)
        {
            switch (Input.Type)
            {
                case InputControlType.None:
                    ControlNone(delta_time, command);
                    break;
                case InputControlType.Direct:
                    ControlDirect(delta_time, command);
                    break;
                case InputControlType.Force:
                    ControlForce(delta_time, command);
                    break;
                case InputControlType.Velocity:
                    ControlVelocity(delta_time, command);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected void ControlNone(double delta_time, InputControlCommand command) { }

        protected void ControlDirect(double delta_time, InputControlCommand command)
        {
            Base.Position += Input.Speed * delta_time * LinearModifiers(command);
        }

        protected void ControlForce(double delta_time, InputControlCommand command)
        {
            var physic = Entity.Get<PhysicComponent>();
            physic.Force += Input.Speed * delta_time * LinearModifiers(command);
        }

        protected void ControlVelocity(double delta_time, InputControlCommand command)
        {
            var physic = Entity.Get<PhysicComponent>();
            physic.Velocity += Input.Speed * delta_time * LinearModifiers(command);
        }

        protected Vector3d LinearModifiers(InputControlCommand command)
        {
            var result = Vector3d.Zero;
            switch (command)
            {
                case InputControlCommand.None:
                    break;
                case InputControlCommand.Forward:
                    result += Base.ForwardRelative;
                    break;
                case InputControlCommand.Backward:
                    result -= Base.ForwardRelative;
                    break;
                case InputControlCommand.StrafeLeft:
                    result -= Base.RightRelative;
                    break;
                case InputControlCommand.StrafeRight:
                    result += Base.RightRelative;
                    break;
                case InputControlCommand.StrafeUp:
                    result += Base.UpRelative;
                    break;
                case InputControlCommand.StrafeDown:
                    result -= Base.UpRelative;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return result;
        }
    }
}
