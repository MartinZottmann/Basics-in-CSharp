using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.Mesh;
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

        protected SizeF size = new SizeF(60, 15);

        protected const double scale = 2;

        public FPSCounter(ResourceManager resources)
            : base(resources)
        {
            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Mesh(
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
            graphic.Mode = BeginMode.Triangles;
            graphic.Program = Resources.Programs["plain_texture"];
        }

        public override void Update(double delta_time, RenderContext render_context)
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

        public override void Render(double delta_time, RenderContext render_context)
        {
            double yMax = render_context.Camera.Near * System.Math.Tan(0.5 * render_context.Camera.Fov);
            double yMin = -yMax;
            double xMin = yMin * render_context.Camera.Aspect;
            double xMax = yMax * render_context.Camera.Aspect;
            Model = Matrix4d.CreateTranslation(0, -size.Height, 0); // Move model top/left to 0, 0
            Model *= Matrix4d.Scale(scale * render_context.Camera.Aspect * MathHelper.TwoPi / render_context.Camera.Fov / (double)render_context.Camera.Width / (double)size.Width);
            Model *= Matrix4d.RotateY(MathHelper.PiOver6);
            Model *= Matrix4d.CreateTranslation(xMin, yMax, -render_context.Camera.Near); // Move model top/left to window top/left

            render_context.Model = Model;
            Resources.Programs["plain_texture"].UniformLocations["PVM"].Set(render_context.Model * render_context.Projection);
            Resources.Programs["plain_texture"].UniformLocations["Texture"].Set(0);
            graphic.Draw();
        }
    }
}
