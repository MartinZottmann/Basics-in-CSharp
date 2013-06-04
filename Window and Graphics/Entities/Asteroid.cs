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
            GL.Translate(Position.X, Position.Y, Position.Z);
            GL.Rotate(Angle, Vector3d.UnitY);
            GL.Begin(BeginMode.Triangles);
            GL.Vertex3(Polygon[0]);
            GL.Vertex3(Polygon[1]);
            GL.Vertex3(Polygon[2]);
            GL.End();
            GL.PopMatrix();

#if DEBUG
            RenderVelocity(delta_time);
#endif
        }
    }
}
