using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Graphics.Shapes;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace MartinZottmann.Game.Entities.GUI
{
    public class Debugger : Entity
    {
        protected SizeF size = new SizeF(300, 30);

        public Debugger(ResourceManager resources)
            : base(resources)
        {
            var shape = new Quad();
            shape.Translate(Matrix4.CreateTranslation(-1, -1, 0) * Matrix4.CreateScale(0.5f * size.Width, 0.5f * size.Height, 0.5f));
            Graphic.Model = new Engine.Graphics.OpenGL.Entity();
            Graphic.Model.Mesh(shape);
            Graphic.Model.Program = Resources.Programs["plain_texture"];
        }

        public override void Update(double delta_time, RenderContext render_context)
        {
            if (Graphic.Model.Texture != null)
                Graphic.Model.Texture.Dispose();
            Graphic.Model.Texture = new Texture(String.Format("Memory: {0}", GC.GetTotalMemory(false)), new Font("Courier", 25, FontStyle.Regular, GraphicsUnit.Pixel, (byte)0), Color.LightGray, Color.FromArgb(127, 255, 255, 255), false, size);
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            double yMax = render_context.Camera.Near * Math.Tan(0.5 * render_context.Camera.Fov);
            double yMin = -yMax;
            double xMin = yMin * render_context.Camera.Aspect;
            double xMax = yMax * render_context.Camera.Aspect;
            Position = new Vector3d(xMax, yMax, -render_context.Camera.Near);
            Scale = new Vector3d(render_context.Camera.Aspect * Math.Tan(0.5 * render_context.Camera.Fov) / MathHelper.TwoPi / render_context.Camera.Width);
            Orientation = new Quaterniond(Vector3d.UnitY, -0.95);

            render_context.Model = Model;
            base.Render(delta_time, render_context);
        }
    }
}
