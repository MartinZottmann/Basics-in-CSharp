using MartinZottmann.Engine;
using MartinZottmann.Graphics;
using MartinZottmann.Graphics.OpenGL;
using MartinZottmann.Math;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace MartinZottmann.Entities
{
    class Textured : Physical
    {
        public Quad quad = Quad.Zero;

        protected string texture_filename = "res/textures/pointer.png";

        protected Texture texture;

        public Textured(Resources resources)
            : base(resources)
        {
            texture = new Texture(texture_filename, false);
        }

        public override void Render(double delta_time)
        {
            GL.PushMatrix();
            using (new Bind(texture))
            {
                GL.Rotate(Angle, Vector3d.UnitY);
                GL.Translate(Position.X, Position.Y, Position.Z);

                GL.Begin(BeginMode.Quads);
                {
                    GL.Color3(Color.Transparent);

                    GL.TexCoord2(0, 1);
                    GL.Vertex3(quad[0]);

                    GL.TexCoord2(1, 1);
                    GL.Vertex3(quad[1]);

                    GL.TexCoord2(1, 0);
                    GL.Vertex3(quad[2]);

                    GL.TexCoord2(0, 0);
                    GL.Vertex3(quad[3]);
                }
                GL.End();
            }
            GL.PopMatrix();

#if DEBUG
            RenderVelocity(delta_time);
#endif
        }
    }
}
