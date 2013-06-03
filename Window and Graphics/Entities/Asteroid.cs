using MartinZottmann.Math;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Entities
{
    class Asteroid : Physical
    {
        public Polygon Polygon = Polygon.Zero;

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

#if DEBUG
            RenderVelocity(delta_time);
#endif
        }
    }
}
