using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace MartinZottmann.Game.Entities.GUI
{
    class FPSCounter : Entity
    {
        protected double accumulator = 0;

        protected int counter = 0;

        protected int fps = 0;

        protected SizeF size;

        public Texture texture;

        public FPSCounter(Resources resources) : base(resources) { }

        public override void Update(double delta_time)
        {
            counter++;
            accumulator += delta_time;
            if (accumulator > 1)
            {
                if (fps != counter)
                {
                    if (texture != null)
                        texture.Dispose();
                    texture = new Texture(String.Format("FPS: {0:F}", counter), new Font("Courier", 9f, FontStyle.Regular, GraphicsUnit.Pixel, (byte)0), Color.White, Color.Transparent, false, out size);
                }
                fps = counter;
                counter = 0;
                accumulator -= 1;
            }
        }

        public override void Render(double delta_time)
        {
            GL.PushMatrix();
            if (texture != null)
                using (new Engine.Bind(texture))
                {
                    GL.Translate(Position.X, Position.Y, Position.Z);

                    GL.Begin(BeginMode.Quads);
                    {
                        GL.Color3(Color.Transparent);

                        GL.TexCoord2(0, 1);
                        GL.Vertex3(0, 0, 0);

                        GL.TexCoord2(1, 1);
                        GL.Vertex3(size.Width, 0, 0);

                        GL.TexCoord2(1, 0);
                        GL.Vertex3(size.Width, size.Height, 0);

                        GL.TexCoord2(0, 0);
                        GL.Vertex3(0, size.Height, 0);

                    }
                    GL.End();
                }
            GL.PopMatrix();
        }
    }
}
