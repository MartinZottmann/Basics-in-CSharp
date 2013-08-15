using MartinZottmann.Game.Graphics;
using OpenTK;

namespace MartinZottmann.Game.Entities.Components
{
    public class Camera : Abstract
    {
        public MartinZottmann.Engine.Graphics.Camera CameraObject;

        protected bool lock_position = false;

        public bool LockPosition
        {
            get { return lock_position; }
            set
            {
                lock_position = value;
                if (lock_position)
                    CameraObject.Position = GameObject.Position;
            }
        }

        protected bool lock_orientation = false;

        public bool LockOrientation
        {
            get { return lock_orientation; }
            set
            {
                lock_orientation = value;
                if (lock_orientation)
                    CameraObject.Orientation = GameObject.Orientation;
            }
        }

        public Camera(GameObject game_object, GameWindow game_window)
            : base(game_object)
        {
            CameraObject = new MartinZottmann.Engine.Graphics.Camera(game_window);
        }

        public override void Update(double delta_time, RenderContext render_context)
        {
            CameraObject.Update(delta_time);
            if (LockPosition)
                CameraObject.Position = GameObject.Position;
            if (LockOrientation)
                CameraObject.Orientation = GameObject.Orientation;
        }
    }
}
