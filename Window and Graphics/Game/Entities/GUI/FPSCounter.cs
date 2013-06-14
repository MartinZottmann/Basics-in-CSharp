using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace MartinZottmann.Game.Entities.GUI
{
    class FPSCounter : Entity
    {
        Engine.Graphics.OpenGL.Entity graphic;

        Matrix4d Model;

        protected double accumulator = 0;

        protected int counter = 0;

        protected int fps = 0;

        protected SizeF size = new SizeF(100, 25);

        public FPSCounter(Resources resources)
            : base(resources)
        {
            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Add(
                new Mesh<VertexP3N3T2, uint>(
                    new VertexP3N3T2[] {
                        new VertexP3N3T2(0, 0, 0, 0, 0, 1, 0, 1),
                        new VertexP3N3T2(size.Width, 0, 0, 0, 0, 1, 1, 1),
                        new VertexP3N3T2(size.Width, size.Height, 0, 0, 0, 1, 1, 0),
                        new VertexP3N3T2(0, size.Height, 0, 0, 0, 1, 0, 0)
                    },
                    new uint[] { 0, 1, 2, 2, 3, 0 }
                )
            );
            graphic.mode = BeginMode.Triangles;
            graphic.Program = Resources.Programs["plain_texture"];

            Model = Matrix4d.Scale(1 / (double)size.Width);
            Model *= Matrix4d.RotateY(MathHelper.PiOver6);
            Model *= Matrix4d.CreateTranslation(-1, 0.5, -2);
        }

        public override void Update(double delta_time)
        {
            counter++;
            accumulator += delta_time;
            if (accumulator > 1)
            {
                if (fps != counter)
                {
                    if (graphic.Texture != null)
                        graphic.Texture.Dispose();
                    graphic.Texture = new Texture(String.Format("FPS: {0:F}", counter), new Font("Courier", 9f, FontStyle.Regular, GraphicsUnit.Pixel, (byte)0), Color.White, Color.Transparent, false, size);
                }
                fps = counter;
                counter = 0;
                accumulator -= 1;
            }
        }

        public override void Render(double delta_time)
        {
            RenderContext.Model = Model;
            Resources.Programs["plain_texture"].UniformLocations["PVM"].Set(RenderContext.Model * RenderContext.Projection);
            Resources.Programs["plain_texture"].UniformLocations["Texture"].Set(0);

            if (graphic.Texture != null)
                graphic.Draw();

            //GL.PushMatrix();
            //if (graphic.Texture != null)
            //    using (new Engine.Bind(graphic.Texture))
            //    {
            //        GL.Translate(Position.X, Position.Y, Position.Z);

            //        GL.Begin(BeginMode.Quads);
            //        {
            //            GL.Color3(Color.Transparent);

            //            GL.TexCoord2(0, 1);
            //            GL.Vertex3(0, 0, 0);

            //            GL.TexCoord2(1, 1);
            //            GL.Vertex3(size.Width, 0, 0);

            //            GL.TexCoord2(1, 0);
            //            GL.Vertex3(size.Width, size.Height, 0);

            //            GL.TexCoord2(0, 0);
            //            GL.Vertex3(0, size.Height, 0);

            //        }
            //        GL.End();
            //    }
            //GL.PopMatrix();
        }
    }
}
