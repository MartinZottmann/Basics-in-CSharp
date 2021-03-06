﻿using MartinZottmann.Engine.Physics;
using OpenTK;
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

        protected double fov = MathHelper.PiOver4;

        public double Fov
        {
            get { return fov; }
            set
            {
                fov = value;
                if (fov <= 0)
                    fov = 0.1;
                if (fov > System.Math.PI)
                    fov = System.Math.PI;
            }
        }

        public double Near = 0.1;

        public double Far = 10000;

        public double Aspect { get { return Window.Width / (double)Window.Height; } }

        public double Width { get { return Window.Width; } }

        public double Height { get { return Window.Height; } }

        public bool InvertXAxis = false;

        public bool InvertYAxis = false;

        public double SensitivityX = 0.1;

        public double SensitivityY = 0.1;

        public GameWindow Window { get; set; }

        public Point WindowCenter { get { return new Point((Window.ClientRectangle.Left + Window.ClientRectangle.Right) / 2, (Window.ClientRectangle.Top + Window.ClientRectangle.Bottom) / 2); } }

        protected Point mouse_position;

        protected bool mouse_look = false;

        public bool MouseLook
        {
            get { return mouse_look; }
            set
            {
                mouse_look = value;
                if (value)
                {
                    mouse_position = Cursor.Position;
                    Cursor.Position = Window.PointToScreen(WindowCenter);
                    Cursor.Hide();
                }
                else
                {
                    Cursor.Show();
                    Cursor.Position = mouse_position;
                }
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
                var mouse_x_delta = window_center.X - Window.Mouse.X;
                var mouse_y_delta = window_center.Y - Window.Mouse.Y;

                if (mouse_x_delta != 0)
                    Orientation = Quaterniond.FromAxisAngle(Up, (InvertXAxis ? -1 : 1) * SensitivityX * mouse_x_delta * delta_time * Fov) * Orientation;
                if (mouse_y_delta != 0)
                    Orientation = Orientation * Quaterniond.FromAxisAngle(Right, (InvertYAxis ? -1 : 1) * SensitivityY * mouse_y_delta * delta_time * Fov);

                // Clamp Orientation
                var pitch = Vector3d.Dot(ForwardRelative, Vector3d.Cross(Up, RightRelative));
                if (pitch < 0)
                {
                    var sign = Vector3d.Dot(Vector3d.Cross(Forward, ForwardRelative), Right) < 0 ? -1 : 1;
                    Orientation = Orientation * Quaterniond.FromAxisAngle(Right, sign * pitch);
                }

                if (Window.WindowState == WindowState.Fullscreen)
                    Cursor.Position = window_center;
                else
                    Cursor.Position = Window.PointToScreen(window_center);
            }
        }

        public Ray3d GetMouseRay()
        {
            var start = new Vector4d((Window.Mouse.X / (double)Window.Width - 0.5) * 2.0, ((Window.Height - Window.Mouse.Y) / (double)Window.Height - 0.5) * 2.0, 0.0, 1.0);
            var end = new Vector4d((Window.Mouse.X / (double)Window.Width - 0.5) * 2.0, ((Window.Height - Window.Mouse.Y) / (double)Window.Height - 0.5) * 2.0, -1.0, 1.0);

            var IPV = ProjectionMatrix.Inverted() * ViewMatrix.Inverted();
            start = Vector4d.Transform(start, IPV);
            start /= start.W;
            end = Vector4d.Transform(end, IPV);
            end /= end.W;
            end -= start;

            return new Ray3d(new Vector3d(start.X, start.Y, start.Z), new Vector3d(end.X, end.Y, end.Z).Normalized());
        }
    }
}
