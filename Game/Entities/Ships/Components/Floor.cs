using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Graphics;
using OpenTK;

namespace MartinZottmann.Game.Entities.Ships.Components
{
    public class Floor : GameObject
    {
        public Floor(ResourceManager resources)
            : base(resources)
        {
            var graphic = AddComponent(new Graphic(this));
            graphic.Model = Resources.Entities.Load("Resources/Models/cube.obj", Matrix4.CreateScale(0.5f, 0.5f, 0.5f));
            var shape = graphic.Model.Mesh();

            graphic.Model.Program = Resources.Programs["standard"];
            graphic.Model.Texture = Resources.Textures["Resources/Textures/debug-256.png"];

            var physic = AddComponent(new Physic(this));
            physic.BoundingBox = shape.BoundingBox;
            physic.BoundingSphere = shape.BoundingSphere;
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            render_context.Model = Model;
            base.Render(delta_time, render_context);
        }
    }
}
