using MartinZottmann.Engine.Resources;

namespace MartinZottmann.Game.Entities
{
    public class Camera : GameObject
    {
        public MartinZottmann.Engine.Graphics.Camera CameraObject;

        public Camera(ResourceManager resources, Window window)
            : base(resources)
        {
            CameraObject = new MartinZottmann.Engine.Graphics.Camera(window);
        }

        public override void Update(double delta_time, Graphics.RenderContext render_context)
        {
            base.Update(delta_time, render_context);

            CameraObject.Position = Position;
            Orientation = CameraObject.Orientation;
        }
    }
}
