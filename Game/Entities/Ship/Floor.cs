using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using OpenTK;
using System;

namespace MartinZottmann.Game.Entities
{
    [Serializable]
    public class Floor : Physical
    {
        public Floor(ResourceManager resources)
            : base(resources)
        {
            graphic = Resources.Entities.Load("Resources/Models/cube.obj", Matrix4.CreateScale(0.5f, 0.5f, 0.5f));
            var shape = graphic.Mesh();

            graphic.Program = Resources.Programs["standard_cube"];
            graphic.Texture = new Texture("Resources/Textures/debug-256.png", false, OpenTK.Graphics.OpenGL.TextureTarget.TextureCubeMap);

            graphic.Program.UniformLocations["in_Texture"].Set(0);
            graphic.Program.UniformLocations["in_LightPosition"].Set(new Vector3(10, 10, 10));

            BoundingBox = shape.BoundingBox;
            BoundingSphere = shape.BoundingSphere;
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            render_context.Model = Model;
            graphic.Program.UniformLocations["in_Model"].Set(render_context.Model);
            graphic.Program.UniformLocations["in_View"].Set(render_context.View);
            graphic.Program.UniformLocations["in_ModelView"].Set(render_context.ViewModel);
            graphic.Program.UniformLocations["in_ModelViewProjection"].Set(render_context.ProjectionViewModel);
            graphic.Program.UniformLocations["in_NormalView"].Set(render_context.Normal);
            base.Render(delta_time, render_context);
        }
    }
}
