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
        public readonly InputManager InputManager;

        public readonly Camera Camera;

        protected readonly Dictionary<Key, InputControlCommand> key_bindings;

        protected NodeList<GameStateNode> game_state_nodes;

        protected NodeList<InputNode> input_nodes;

        public InputSystem(InputManager input_manager, Camera camera)
        {
            InputManager = input_manager;
            InputManager.KeyUp += OnKeyboardKeyUp;
            InputManager.ButtonUp += OnMouseButtonUp;

            Camera = camera;

            key_bindings = new Dictionary<Key, InputControlCommand>();
            key_bindings.Add(Key.W, InputControlCommand.Forward);
            key_bindings.Add(Key.S, InputControlCommand.Backward);
            key_bindings.Add(Key.A, InputControlCommand.StrafeLeft);
            key_bindings.Add(Key.D, InputControlCommand.StrafeRight);
            key_bindings.Add(Key.Space, InputControlCommand.StrafeUp);
            key_bindings.Add(Key.ShiftLeft, InputControlCommand.StrafeDown);
        }

        public void Start(EntityManager entity_manager)
        {
            game_state_nodes = entity_manager.GetNodeList<GameStateNode>();
            input_nodes = entity_manager.GetNodeList<InputNode>();
        }

        public void Update(double delta_time)
        {
            foreach (var game_state_node in game_state_nodes)
                foreach (var input_node in input_nodes)
                    if (game_state_node.GameState.input_entity == input_node.Entity)
                        foreach (var key_binding in key_bindings)
                            if (InputManager[key_binding.Key])
                                input_node.Control(delta_time, key_binding.Value);

            if (InputManager[Key.KeypadPlus])
                Camera.Fov += MathHelper.PiOver6 * delta_time;
            if (InputManager[Key.KeypadSubtract])
                Camera.Fov -= MathHelper.PiOver6 * delta_time;
        }

        public void Render(double delta_time) { }

        public void Stop()
        {
            game_state_nodes = null;
            input_nodes = null;
        }

        protected void OnKeyboardKeyUp(object sender, InputKeyboardEventArgs e)
        {
            if (e.Handled)
                return;

            if (e.Key == Key.F3)
                foreach (var game_state_node in game_state_nodes)
                {
                    game_state_node.GameState.Debug = !game_state_node.GameState.Debug;
                    e.Handled = true;
                }
        }

        protected void OnMouseButtonUp(object sender, InputMouseEventArgs e)
        {
            if (e.Handled)
                return;

            if (e.Button == MouseButton.Right)
            {
                Camera.MouseLook = !Camera.MouseLook;
                e.Handled = true;
            }
        }
    }
}
