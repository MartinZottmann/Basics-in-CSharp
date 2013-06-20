using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Graphics.Shapes;
using MartinZottmann.Engine.Resources;
using OpenTK;

namespace MartinZottmann.Game.Entities
{
    class Floor : Physical
    {
        Engine.Graphics.OpenGL.Entity graphic;

        public Floor(ResourceManager resources)
            : base(resources)
        {
            Scale = new Vector3d(1, 0.1, 1);
            Offset.Y -= 0.9;

            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Add(new CubeHardNormals());
            graphic.Program = Resources.Programs["standard_cube"];
            var texture = new Texture("res/textures/debug-256.png", false, OpenTK.Graphics.OpenGL.TextureTarget.TextureCubeMap);
            graphic.Texture = texture;

            graphic.Program.AddUniformLocation("in_Texture").Set(0);
            graphic.Program.AddUniformLocation("in_Model");
            graphic.Program.AddUniformLocation("in_View");
            graphic.Program.AddUniformLocation("in_ModelView");
            graphic.Program.AddUniformLocation("in_ModelViewProjection");
            graphic.Program.AddUniformLocation("in_NormalView");
            graphic.Program.AddUniformLocation("in_LightPosition").Set(new Vector3(10, 10, 10));

            BoundingBox.Max = new Vector3d(1, 1, 1);
            BoundingBox.Min = new Vector3d(-1, -1, -1);
        }

        public override void Render(double delta_time)
        {
            graphic.Program.UniformLocations["in_Model"].Set(RenderContext.Model);
            graphic.Program.UniformLocations["in_View"].Set(RenderContext.View);
            graphic.Program.UniformLocations["in_ModelView"].Set(RenderContext.ViewModel);
            graphic.Program.UniformLocations["in_ModelViewProjection"].Set(RenderContext.ProjectionViewModel);
            graphic.Program.UniformLocations["in_NormalView"].Set(RenderContext.Normal);
            graphic.Draw();
        }
    }
}
