using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics;
using MartinZottmann.Game.Entities.Nodes;
using OpenTK;
using OpenTK.Input;

namespace MartinZottmann.Game.Entities.Systems
{
    public class InputSystem : ISystem
    {
        public Camera Camera;

        public GameWindow Window;

        protected NodeList<InputNode> input_nodes;

        public InputSystem(GameWindow window, Camera camera)
        {
            Window = window;
            Camera = camera;
        }

        public void Bind(EntityManager entitiy_manager)
        {
            input_nodes = entitiy_manager.Get<InputNode>();
        }

        public void Update(double delta_time)
        {
            Camera.Update(delta_time);

            foreach (var input_node in input_nodes)
            {
                var window = Camera.Window;
                var forward = Camera.MouseLook
                    ? Camera.ForwardRelative
                    : (Camera.ForwardRelative - Vector3d.Dot(Camera.ForwardRelative, Camera.Up) * Camera.Up).Normalized();

                if (window.Keyboard[Key.W])
                    Camera.Position += forward * delta_time * 100;
                if (window.Keyboard[Key.S])
                    Camera.Position -= forward * delta_time * 100;
                if (window.Keyboard[Key.A])
                    Camera.Position -= Camera.RightRelative * delta_time * 100;
                if (window.Keyboard[Key.D])
                    Camera.Position += Camera.RightRelative * delta_time * 100;
                if (window.Keyboard[Key.Space])
                    Camera.Position += Camera.UpRelative * delta_time * 100;
                if (window.Keyboard[Key.ShiftLeft])
                    Camera.Position -= Camera.UpRelative * delta_time * 100;
                if (window.Keyboard[Key.KeypadPlus])
                    Camera.Fov += MathHelper.PiOver6 * delta_time;
                if (window.Keyboard[Key.KeypadSubtract])
                    Camera.Fov -= MathHelper.PiOver6 * delta_time;

                //if (Window.Keyboard[Key.J])
                //    foreach (var entity in selection)
                //    {
                //        entity.GetComponent<Physic>().AddForceRelative(new Vector3d(0, 0, -1), new Vector3d(10, 0, 0) * delta_time);
                //        entity.GetComponent<Physic>().AddForceRelative(new Vector3d(0, 0, 1), new Vector3d(-10, 0, 0) * delta_time);
                //    }
                //if (Window.Keyboard[Key.L])
                //    foreach (var entity in selection)
                //    {
                //        entity.GetComponent<Physic>().AddForceRelative(new Vector3d(0, 0, -1), new Vector3d(-10, 0, 0) * delta_time);
                //        entity.GetComponent<Physic>().AddForceRelative(new Vector3d(0, 0, 1), new Vector3d(10, 0, 0) * delta_time);
                //    }
                //if (Window.Keyboard[Key.I])
                //    foreach (var entity in selection)
                //    {
                //        entity.GetComponent<Physic>().AddForceRelative(new Vector3d(0, 0, -1), new Vector3d(0, 10, 0) * delta_time);
                //        entity.GetComponent<Physic>().AddForceRelative(new Vector3d(0, 0, 1), new Vector3d(0, -10, 0) * delta_time);
                //    }
                //if (Window.Keyboard[Key.K])
                //    foreach (var entity in selection)
                //    {
                //        entity.GetComponent<Physic>().AddForceRelative(new Vector3d(0, 0, -1), new Vector3d(0, -10, 0) * delta_time);
                //        entity.GetComponent<Physic>().AddForceRelative(new Vector3d(0, 0, 1), new Vector3d(0, 10, 0) * delta_time);
                //    }
                //if (Window.Keyboard[Key.U])
                //    foreach (var entity in selection)
                //    {
                //        entity.GetComponent<Physic>().AddForceRelative(new Vector3d(-1, 0, 0), new Vector3d(0, -10, 0) * delta_time);
                //        entity.GetComponent<Physic>().AddForceRelative(new Vector3d(1, 0, 0), new Vector3d(0, 10, 0) * delta_time);
                //    }
                //if (Window.Keyboard[Key.O])
                //    foreach (var entity in selection)
                //    {
                //        entity.GetComponent<Physic>().AddForceRelative(new Vector3d(-1, 0, 0), new Vector3d(0, 10, 0) * delta_time);
                //        entity.GetComponent<Physic>().AddForceRelative(new Vector3d(1, 0, 0), new Vector3d(0, -10, 0) * delta_time);
                //    }
                //if (Window.Keyboard[Key.M])
                //    foreach (var entity in selection)
                //        entity.GetComponent<Physic>().AngularVelocity = Vector3d.Zero;

                input_node.Base.Position = Camera.Position;
            }
        }

        public void Render(double delta_time)
        {
            // void
        }
    }
}
