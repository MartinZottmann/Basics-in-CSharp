using MartinZottmann.Engine.Graphics.Shapes;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Graphics;
using OpenTK;

namespace MartinZottmann.Game.Entities
{
    class Textured : Physical
    {
        public Textured(ResourceManager resources)
            : base(resources)
        {
            Position = new Vector3d(-10, -10, -10);

            var shape = new Quad();
            shape.Translate(Matrix4.CreateScale(2) * Matrix4.CreateRotationX(-MathHelper.PiOver2));

            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Mesh(shape);
            graphic.Program = Resources.Programs["plain_texture"];
            graphic.Texture = Resources.Textures["Resources/Textures/pointer.png"];

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
