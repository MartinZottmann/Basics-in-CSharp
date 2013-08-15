using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
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

        protected FontMeshBuilder font_mesh_builder;

        protected World world;

        public Debugger(ResourceManager resources, Texture font_texture, FontMeshBuilder font_mesh_builder, World world)
            : base(resources)
        {
            this.font_mesh_builder = font_mesh_builder;
            this.world = world;

            Scale = new Vector3d(2);

            var graphic = AddComponent(new Graphic(this));
            graphic.Model = new Engine.Graphics.OpenGL.Entity();
            graphic.Model.Program = Resources.Programs["plain_texture"];
            graphic.Model.Texture = font_texture;
        }

        public override void Update(double delta_time, RenderContext render_context)
        {
            if (!render_context.Debug)
                return;

            var graphic = GetComponent<Graphic>();
            graphic.Model.Mesh(
                font_mesh_builder.FromString(
                    String.Format(
                        "Memory: {0}\nWorld objects: {1}\nEntities: {2}\nPrograms: {3}\nShaders: {4}\nTextures: {5}\nWavefrontObjFiles: {6}",
                        GC.GetTotalMemory(false),
                        world.Children.Count,
                        Resources.Entities.Count,
                        Resources.Programs.Count,
                        Resources.Shaders.Count,
                        Resources.Textures.Count,
                        Resources.WavefrontObjFiles.Count
                    )
                )
            );
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            if (!render_context.Debug)
                return;

            // @todo Remove once keyboard inputs are thread safe so render_context.Debug cannot change between Update and Render
            var graphic = GetComponent<Graphic>();
            if (graphic.Model.Mesh() == null)
                return;

            Position = new Vector3d(-0.90, 0.5, -1) * render_context.Projection.Inverted();
            base.Render(delta_time, render_context);
        }
    }
}
