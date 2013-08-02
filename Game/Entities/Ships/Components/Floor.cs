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
            Graphic.Model = Resources.Entities.Load("Resources/Models/cube.obj", Matrix4.CreateScale(0.5f, 0.5f, 0.5f));
            var shape = Graphic.Model.Mesh();

            Graphic.Model.Program = Resources.Programs["standard"];
            Graphic.Model.Texture = Resources.Textures["Resources/Textures/debug-256.png"];

            Physic.BoundingBox = shape.BoundingBox;
            Physic.BoundingSphere = shape.BoundingSphere;
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            render_context.Model = Model;
            base.Render(delta_time, render_context);
        }
    }
}
