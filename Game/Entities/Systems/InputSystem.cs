using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics;
using MartinZottmann.Game.Entities.Nodes;
using MartinZottmann.Game.Input;
using OpenTK;
using OpenTK.Input;
using System.Collections.Generic;

namespace MartinZottmann.Game.Entities.Systems
{
    public class InputSystem : ISystem
    {
        public Camera Camera;

        public GameWindow Window;

        protected Dictionary<Key, InputControlCommand> key_bindings = new Dictionary<Key, InputControlCommand>();

        protected NodeList<InputNode> input_nodes;

        public InputSystem(GameWindow window, Camera camera)
        {
            Window = window;
            Camera = camera;

            key_bindings.Add(Key.W, InputControlCommand.Forward);
            key_bindings.Add(Key.S, InputControlCommand.Backward);
            key_bindings.Add(Key.A, InputControlCommand.StrafeLeft);
            key_bindings.Add(Key.D, InputControlCommand.StrafeRight);
            key_bindings.Add(Key.Space, InputControlCommand.StrafeUp);
            key_bindings.Add(Key.ShiftLeft, InputControlCommand.StrafeDown);
        }

        public void Start(EntityManager entity_manager)
        {
            input_nodes = entity_manager.GetNodeList<InputNode>();
        }

        public void Update(double delta_time)
        {
            foreach (var input_node in input_nodes)
                foreach (var key_binding in key_bindings)
                    if (Window.Keyboard[key_binding.Key])
                        input_node.Control(delta_time, key_binding.Value);

            if (Window.Keyboard[Key.KeypadPlus])
                Camera.Fov += MathHelper.PiOver6 * delta_time;
            if (Window.Keyboard[Key.KeypadSubtract])
                Camera.Fov -= MathHelper.PiOver6 * delta_time;
        }

        public void Render(double delta_time) { }

        public void Stop()
        {
            input_nodes = null;
        }
    }
}
