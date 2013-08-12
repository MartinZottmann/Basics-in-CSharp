using MartinZottmann.Engine.Graphics.Shapes;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Graphics;
using OpenTK;

namespace MartinZottmann.Game.Entities
{
    class Textured : GameObject
    {
        public Textured(ResourceManager resources)
            : base(resources)
        {
            var shape = new Quad();
            shape.Translate(Matrix4.CreateScale(2) * Matrix4.CreateRotationX(-MathHelper.PiOver2));

            var graphic = AddComponent(new Graphic(this));
            graphic.Model = new Engine.Graphics.OpenGL.Entity();
            graphic.Model.Mesh(shape);
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
