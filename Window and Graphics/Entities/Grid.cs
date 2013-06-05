using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace MartinZottmann.Entities
{
    class Grid : Entity
    {
        public Grid()
            : base()
        {
            color = Color.LightGray;
        }

        public override void Render(double delta_time)
        {
            GL.PushMatrix();
            {
                GL.PointSize(1);

                // Axis
                GL.Begin(BeginMode.Lines);
                {
                    GL.Color3(color);
                    GL.Vertex3(-1000, 0, 0);
                    GL.Vertex3(1000, 0, 0);
                    GL.Vertex3(0, -1000, 0);
                    GL.Vertex3(0, 1000, 0);
                    GL.Vertex3(0, 0, -1000);
                    GL.Vertex3(0, 0, 1000);
                }
                GL.End();

                GL.Begin(BeginMode.LineLoop);
                {
                    GL.Color3(color);

                    for (int i = 0; i <= 360; i++)
                    {
                        double angle = 2 * System.Math.PI * i / 360;
                        GL.Vertex3(100 * System.Math.Cos(angle), 0, 100 * System.Math.Sin(angle));
                    }
                }
                GL.End();
            }
            GL.PopMatrix();
        }
    }
}
