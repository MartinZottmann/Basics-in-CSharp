using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann
{
    class Asteroid : SuperBall
    {
        public Polygon Polygon = Polygon.Zero;

        public double Angle = 0;

        public double AngleVelocity = 100;

        public override void Update(double delta_time)
        {
            base.Update(delta_time);

            Angle += AngleVelocity * delta_time;
            if (Angle > 359)
            {
                Angle -= 360;
            }
        }

        public override void Render(double delta_time)
        {
            GL.PushMatrix();
            GL.Color3(color);
            GL.Translate(Position.X, Position.Y, 0);
            GL.Rotate(Angle, Vector3d.UnitZ);
            GL.Begin(BeginMode.Triangles);
            foreach (var v in Polygon.points)
            {
                GL.Vertex2(v);
            }
            GL.End();
            GL.PopMatrix();
        }
    }
}
