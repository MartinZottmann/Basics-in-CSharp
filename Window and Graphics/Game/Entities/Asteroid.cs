using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Graphics.Shapes;
using MartinZottmann.Engine.Resources;
using OpenTK;

namespace MartinZottmann.Game.Entities
{
    class Asteroid : Physical
    {
        Engine.Graphics.OpenGL.Entity graphic;

        public Asteroid(ResourceManager resources)
            : base(resources)
        {
            Position = new Vector3d(
                (randomNumber.NextDouble() - 0.5) * 100.0,
                (randomNumber.NextDouble() - 0.5) * 100.0,
                (randomNumber.NextDouble() - 0.5) * 100.0
            );

            var scale = randomNumber.NextDouble() * 5 + 1;

            Mass *= scale;

            var shape = new Sphere();
            shape.Translate(Matrix4.Scale((float)scale));

            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Mesh(shape);
            graphic.Program = Resources.Programs["standard_cube"];
            var texture = new Texture("res/textures/debug-256.png", false, OpenTK.Graphics.OpenGL.TextureTarget.TextureCubeMap);
            graphic.Texture = texture;

            graphic.Program.UniformLocations["in_Texture"].Set(0);
            graphic.Program.UniformLocations["in_LightPosition"].Set(new Vector3(10, 10, 10));

            BoundingBox = shape.BoundingBox;
        }

        public override void Update(double delta_time)
        {
            Force = new Vector3d(
                (randomNumber.NextDouble() - 0.5) * 10000.0 * delta_time,
                (randomNumber.NextDouble() - 0.5) * 10000.0 * delta_time,
                (randomNumber.NextDouble() - 0.5) * 10000.0 * delta_time
            );

            base.Update(delta_time);
        }

        public override void Render(double delta_time)
        {
            graphic.Program.UniformLocations["in_Model"].Set(RenderContext.Model);
            graphic.Program.UniformLocations["in_View"].Set(RenderContext.View);
            graphic.Program.UniformLocations["in_ModelView"].Set(RenderContext.ViewModel);
            graphic.Program.UniformLocations["in_ModelViewProjection"].Set(RenderContext.ProjectionViewModel);
            graphic.Program.UniformLocations["in_NormalView"].Set(RenderContext.Normal);
            graphic.Draw();

#if DEBUG
            RenderVelocity(delta_time);
            RenderBoundingBox(delta_time);
#endif
        }
    }
}
