using OpenTK;
using OpenTK.Input;

namespace MartinZottmann.Game.Entities.Components
{
    public class Input : Abstract
    {
        public Input(GameObject game_object) : base(game_object) { }

        public override void Update(double delta_time, Graphics.RenderContext render_context)
        {
            var window = render_context.Window;
            if (window.Keyboard[Key.W])
                GameObject.Position += GameObject.ForwardRelative * delta_time * 100;
            if (window.Keyboard[Key.S])
                GameObject.Position -= GameObject.ForwardRelative * delta_time * 100;
            if (window.Keyboard[Key.A])
                GameObject.Position -= GameObject.RightRelative * delta_time * 100;
            if (window.Keyboard[Key.D])
                GameObject.Position += GameObject.RightRelative * delta_time * 100;
            if (window.Keyboard[Key.Space])
                GameObject.Position += GameObject.UpRelative * delta_time * 100;
            if (window.Keyboard[Key.ShiftLeft])
                GameObject.Position -= GameObject.UpRelative * delta_time * 100;
            if (window.Keyboard[Key.KeypadPlus])
                render_context.Camera.Fov += MathHelper.PiOver6 * delta_time;
            if (window.Keyboard[Key.KeypadSubtract])
                render_context.Camera.Fov -= MathHelper.PiOver6 * delta_time;
            if (render_context.Camera.Fov <= 0)
                render_context.Camera.Fov = 0.1;
            if (render_context.Camera.Fov > System.Math.PI)
                render_context.Camera.Fov = System.Math.PI;
        }
    }
}
