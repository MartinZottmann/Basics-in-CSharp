using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace MartinZottmann.Entities.GUI
{
    class FPSCounter : Entity
    {
        protected double accumulator = 0;

        protected int counter = 0;

        protected int fps = 0;

        protected SizeF size;

        protected int texture_id;

        public FPSCounter() : base() { }

        public override void Update(double delta_time)
        {
            counter++;
            accumulator += delta_time;
            if (accumulator > 1)
            {
                if (fps != counter)
                {
                    if (texture_id != 0)
                    {
                        GL.DeleteTexture(texture_id);
                    }
                    //Console.WriteLine("FPS: {0:F}", counter);
                    texture_id = MartinZottmann.Graphics.LoadTexture(String.Format("FPS: {0:F}", counter), new Font("Courier", 9f, FontStyle.Regular, GraphicsUnit.Pixel, (byte)0), Color.White, Color.Transparent, false, out size);
                }
                fps = counter;
                counter = 0;
                accumulator -= 1;
            }
        }

        public override void Render(double delta_time)
        {
            GL.PushMatrix();
            GL.Color3(Color.Transparent);
            GL.Translate(Position.X, Position.Y, Position.Z);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texture_id);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0, 0);
            GL.Vertex3(0, size.Height, 0);

            GL.TexCoord2(1, 0);
            GL.Vertex3(size.Width, size.Height, 0);

            GL.TexCoord2(1, 1);
            GL.Vertex3(size.Width, 0, 0);

            GL.TexCoord2(0, 1);
            GL.Vertex3(0, 0, 0);

            GL.End();
            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();
        }
    }
}
