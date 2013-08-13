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
    public class Debugger : GameObject
    {
        protected SizeF size = new SizeF(300, 30);

        private FontMeshBuilder font_mesh_builder;

        public Debugger(ResourceManager resources, Texture font_texture, FontMeshBuilder font_mesh_builder)
            : base(resources)
        {
            this.font_mesh_builder = font_mesh_builder;

            Scale = new Vector3d(2);

            var graphic = AddComponent(new Graphic(this));
            graphic.Model = new Engine.Graphics.OpenGL.Entity();
            graphic.Model.Program = Resources.Programs["plain_texture"];
            graphic.Model.Texture = font_texture;
        }

        public override void Update(double delta_time, RenderContext render_context)
        {
            var graphic = GetComponent<Graphic>();
            graphic.Model.Mesh(font_mesh_builder.FromString(String.Format("Memory: {0}", GC.GetTotalMemory(false))));
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            Position = new Vector3d(-0.75, 0.5, -1) * render_context.Projection.Inverted();
            base.Render(delta_time, render_context);
        }
    }
}
