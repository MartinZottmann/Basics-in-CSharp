using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MartinZottmann
{
    public class Camera
    {
        public Vector3d Position = Vector3d.Zero;

        public Vector3d Direction = Vector3d.UnitZ;

        public Vector3d LookAt
        {
            get
            {
                return Position - Direction;
            }
        }

        public Vector3d Up = Vector3d.UnitY;

        public Vector3d Right
        {
            get
            {
                var right = Vector3d.Cross(Direction, Up);
                right.NormalizeFast();
                return right;
            }
        }

        public float Fov = MathHelper.PiOver4;

        public float Near = 0.1f;

        public float Far = 1000.0f;

        GameWindow Window { get; set; }

        protected Point WindowCenter
        {
            get
            {
                return new Point((Window.ClientRectangle.Left + Window.ClientRectangle.Right) / 2, (Window.ClientRectangle.Top + Window.ClientRectangle.Bottom) / 2);
            }
        }

        bool mouse_look = false;

        public bool MouseLook
        {
            get
            {
                return mouse_look;
            }
            set
            {
                mouse_look = value;
                if (value)
                {
                    Cursor.Hide();
                }
                else
                {
                    Cursor.Show();
                }
            }
        }

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
                {
                    RotateDirectionAroundUp(-mouse_x_delta * delta_time * 0.1);
                }
                if (mouse_y_delta != 0)
                {
                    RotateDirectionAroundRight(mouse_y_delta * delta_time * 0.1);
                }

                if (Window.WindowState == WindowState.Fullscreen)
                {
                    Cursor.Position = window_center;
                }
                else
                {
                    Cursor.Position = Window.PointToScreen(window_center);
                }
            }
        }

        public Matrix4d ProjectionMatrix()
        {
            return Matrix4d.CreatePerspectiveFieldOfView(Fov, Window.Width / (float)Window.Height, Near, Far);
        }

        public Matrix4d ModelviewMatrix()
        {
            return Matrix4d.LookAt(Position, LookAt, Up);
        }

        public void RotateDirectionAroundUp(double angle)
        {
            var matrix = Matrix4d.CreateTranslation(-Position);
            matrix = Matrix4d.Rotate(Up, angle);
            Vector3d.Transform(ref Direction, ref matrix, out Direction);
        }

        public void RotateDirectionAroundRight(double angle)
        {
            var matrix = Matrix4d.CreateTranslation(-Position);
            matrix = Matrix4d.Rotate(Right, angle);
            Vector3d.Transform(ref Direction, ref matrix, out Direction);
        }
    }
}
