using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Graphics.Shapes;
using MartinZottmann.Engine.Resources;
using OpenTK;

namespace MartinZottmann.Game.Entities
{
    class Floor : Physical
    {
        public Floor(ResourceManager resources)
            : base(resources)
        {
            var shape = new CubeHardNormals();
            shape.Translate(Matrix4.CreateScale(0.5f) * Matrix4.CreateTranslation(new Vector3(0, -1, 0)));

            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Mesh(shape);
            graphic.Program = Resources.Programs["standard_cube"];
            graphic.Texture = new Texture("Resources/Textures/debug-256.png", false, OpenTK.Graphics.OpenGL.TextureTarget.TextureCubeMap);

            graphic.Program.UniformLocations["in_Texture"].Set(0);
            graphic.Program.UniformLocations["in_LightPosition"].Set(new Vector3(10, 10, 10));

            BoundingBox.Max = new Vector3d(0.5, 1.5, 0.5);
            BoundingBox.Min = new Vector3d(-0.5, -1.5, -0.5);
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
            graphic.Draw();
        }
    }
}
