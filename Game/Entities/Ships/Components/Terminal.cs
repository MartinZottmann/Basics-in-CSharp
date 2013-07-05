using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Graphics;
using OpenTK;

namespace MartinZottmann.Game.Entities.Ships.Components
{
    public class Terminal : Component
    {
        public Terminal(ResourceManager resources)
            : base(resources)
        {
            graphic = Resources.Entities.Load("Resources/Models/table.obj", Matrix4.CreateScale(0.5f, 0.5f, 0.5f));
            var shape = graphic.Mesh();

            graphic.Program = Resources.Programs["standard"];
            graphic.Texture = Resources.Textures["Resources/Textures/debug-256.png"];

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
