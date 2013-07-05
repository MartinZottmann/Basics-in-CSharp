using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Graphics;
using OpenTK;

namespace MartinZottmann.Game.Entities.Ships.Components
{
    public class Floor : Component
    {
        public Floor(ResourceManager resources)
            : base(resources)
        {
            graphic = Resources.Entities.Load("Resources/Models/cube.obj", Matrix4.CreateScale(0.5f, 0.5f, 0.5f));
            var shape = graphic.Mesh();

            graphic.Program = Resources.Programs["standard"];
            graphic.Texture = Resources.Textures["Resources/Textures/debug-256.png"];

            BoundingBox = shape.BoundingBox;
            BoundingSphere = shape.BoundingSphere;
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            render_context.Model = Model;
            base.Render(delta_time, render_context);
        }
    }
}
