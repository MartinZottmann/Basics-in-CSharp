using OpenTK;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MartinZottmann.Engine.Graphics
{
    public class Camera
    {
        public Vector3d Position = Vector3d.Zero;

        public Quaterniond Orientation = Quaterniond.Identity;

        public Vector3d Forward = -Vector3d.UnitZ;

        public Vector3d ForwardRelative { get { return Vector3d.Transform(Forward, Orientation); } }

        public Vector3d Up = Vector3d.UnitY;

        public Vector3d UpRelative { get { return Vector3d.Transform(Up, Orientation); } }

        public Vector3d Right = Vector3d.UnitX;

        public Vector3d RightRelative { get { return Vector3d.Transform(Right, Orientation); } }

        public Vector3d LookAt
        {
            get { return Position + ForwardRelative; }
            set { Orientation = Matrix4d.LookAt(Position, value, Up).ExtractRotation().Inverted(); }
        }

        public double Fov = MathHelper.PiOver4;

        public double Near = 0.1;

        public double Far = 10000;

        public double Aspect { get { return Window.Width / (double)Window.Height; } }

        public double Width { get { return Window.Width; } }

        public double Height { get { return Window.Height; } }

        public bool InvertXAxis = false;

        public bool InvertYAxis = true;

        public double SensitivityX = 0.1;

        public double SensitivityY = 0.1;

        public GameWindow Window { get; set; }

        public Point WindowCenter { get { return new Point((Window.ClientRectangle.Left + Window.ClientRectangle.Right) / 2, (Window.ClientRectangle.Top + Window.ClientRectangle.Bottom) / 2); } }

        bool mouse_look = false;

        public bool MouseLook
        {
            get { return mouse_look; }
            set
            {
                mouse_look = value;
                if (value)
                    Cursor.Hide();
                else
                    Cursor.Show();
            }
        }

        public Matrix4d ProjectionMatrix { get { return Matrix4d.CreatePerspectiveFieldOfView(Fov, Aspect, Near, Far); } }

        public Matrix4d ViewMatrix { get { return Matrix4d.LookAt(Position, LookAt, UpRelative); } }

        public Camera(GameWindow window)
        {
            Window = window;
        }

        public void Update(double delta_time)
        {
            if (MouseLook)
            {
                var window_center = WindowCenter;
                var mouse_x_delta = Window.Mouse.X - window_center.X;
                var mouse_y_delta = Window.Mouse.Y - window_center.Y;

                if (mouse_x_delta != 0)
                    Orientation = Quaterniond.FromAxisAngle(Up, (InvertXAxis ? 1 : -1) * SensitivityX * mouse_x_delta * delta_time * Fov) * Orientation;
                if (mouse_y_delta != 0)
                    Orientation = Orientation * Quaterniond.FromAxisAngle(Right, (InvertYAxis ? 1 : -1) * SensitivityY * mouse_y_delta * delta_time * Fov);

                if (Window.WindowState == WindowState.Fullscreen)
                    Cursor.Position = window_center;
                else
                    Cursor.Position = Window.PointToScreen(window_center);
            }
        }
    }
}
