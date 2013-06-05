using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann
{
    public class Camera
    {
        public Vector3d Position = Vector3d.Zero;

        public Vector3d LookAt = Vector3d.UnitZ;

        public Vector3d Up = Vector3d.UnitY;

        public float Fov = MathHelper.PiOver4;

        public float Near = 0.1f;

        public float Far = 1000.0f;

        GameWindow Window { get; set; }

        public Camera(GameWindow window)
        {
            Window = window;
        }

        public Matrix4d ProjectionMatrix()
        {
            return Matrix4d.CreatePerspectiveFieldOfView(Fov, Window.Width / (float)Window.Height, Near, Far);
        }

        public Matrix4d ModelviewMatrix()
        {
            return Matrix4d.LookAt(Position, Position + LookAt, Up);
        }

        public void RotateLookAtAroundUp(double angle)
        {
            var matrix = Matrix4d.CreateTranslation(-Position);
            matrix = Matrix4d.Rotate(Up, angle);
            Vector3d.Transform(ref LookAt, ref matrix, out LookAt);
        }

        public void RotateLookAtAroundRight(double angle)
        {
            var right = Vector3d.Cross(Position, Up);
            var matrix = Matrix4d.CreateTranslation(-Position);
            matrix = Matrix4d.Rotate(right, angle);
            Vector3d.Transform(ref LookAt, ref matrix, out LookAt);
        }
    }
}
