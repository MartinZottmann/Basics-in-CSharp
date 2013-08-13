using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Graphics.Shapes;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Graphics;
using OpenTK;
using System;
using System.Drawing;

namespace MartinZottmann.Game.Entities.GUI
{
    class FPSCounter : GameObject
    {
        protected double accumulator = 0;

        protected int counter = 0;

        protected int fps = 0;

        protected SizeF size = new SizeF(300, 30);

        protected FontMeshBuilder font_mesh_builder;

        public FPSCounter(ResourceManager resources, Texture font_texture, FontMeshBuilder font_mesh_builder)
            : base(resources)
        {
            this.font_mesh_builder = font_mesh_builder;

            Scale = new Vector3d(2);

            var graphic = AddComponent(new Graphic(this));
            graphic.Model = new Engine.Graphics.OpenGL.Entity();
            graphic.Model.Mesh(font_mesh_builder.FromString(String.Format("FPS: {0:F}", counter)));
            graphic.Model.Program = Resources.Programs["plain_texture"];
            graphic.Model.Texture = font_texture;
        }

        public override void Update(double delta_time, RenderContext render_context)
        {
            counter++;
            accumulator += delta_time;
            if (accumulator > 1)
            {
                if (fps != counter)
                {
                    var graphic = GetComponent<Graphic>();
                    graphic.Model.Mesh(font_mesh_builder.FromString(String.Format("FPS: {0:F}", counter)));
                }
                fps = counter;
                counter = 0;
                accumulator -= 1;
            }
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            Position = new Vector3d(-0.75, 0.75, -1) * render_context.Projection.Inverted();
            base.Render(delta_time, render_context);
        }
    }
}
