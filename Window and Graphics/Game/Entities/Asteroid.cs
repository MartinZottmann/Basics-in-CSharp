using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Graphics.Shapes;
using MartinZottmann.Engine.Resources;
using OpenTK;

namespace MartinZottmann.Game.Entities
{
    class Asteroid : Physical
    {
        Engine.Graphics.OpenGL.Entity graphic;

        public Asteroid(Resources resources)
            : base(resources)
        {
            Position = new Vector3d(
                (randomNumber.NextDouble() - 0.5) * 100.0,
                (randomNumber.NextDouble() - 0.5) * 100.0,
                (randomNumber.NextDouble() - 0.5) * 100.0
            );

            Scale = (float)(randomNumber.NextDouble() * 5 + 1);

            Mass *= Scale;

            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Add(new CubeHardNormals());
            graphic.Program = Resources.Programs["standard_cube"];
            var texture = new Texture("res/textures/debug-256.png", false, OpenTK.Graphics.OpenGL.TextureTarget.TextureCubeMap);
            graphic.Texture = texture;

            graphic.Program.AddUniformLocation("in_Texture").Set(0);
            //in_texture.Set(graphic.texture.id);

            graphic.Program.AddUniformLocation("in_Model");
            graphic.Program.AddUniformLocation("in_View");
            //graphic.program.AddUniformLocation("in_Projection");
            graphic.Program.AddUniformLocation("in_ModelView");
            //graphic.program.AddUniformLocation("in_ViewProjection");
            graphic.Program.AddUniformLocation("in_ModelViewProjection");
            //graphic.program.AddUniformLocation("in_AmbientColor").Set(new OpenTK.Graphics.Color4(0, 0, 0, 255));
            //graphic.program.AddUniformLocation("in_DiffuseColor").Set(new OpenTK.Graphics.Color4(255, 255, 255, 255));
            //graphic.program.AddUniformLocation("in_SpecularColor").Set(new OpenTK.Graphics.Color4(127, 127, 127, 255));
            //graphic.program.AddUniformLocation("in_AmbientLight").Set(new OpenTK.Graphics.Color4(127, 127, 127, 255));
            //NormalMatrixUniform = graphic.program.AddUniformLocation("in_NormalMatrix");
            graphic.Program.AddUniformLocation("in_NormalView");
            //graphic.program.AddUniformLocation("in_LightColor").Set(new OpenTK.Graphics.Color4(127, 127, 127, 255));
            graphic.Program.AddUniformLocation("in_LightPosition").Set(new Vector3(10, 10, 10));
            //graphic.program.AddUniformLocation("in_Shininess").Set(100f);
            //graphic.program.AddUniformLocation("in_Strength").Set(0.1f);
            //graphic.program.AddUniformLocation("in_EyeDirection");
            //graphic.program.AddUniformLocation("in_ConstantAttenuation").Set(0.1f);
            //graphic.program.AddUniformLocation("in_LinearAttenuation").Set(0.1f);
            //graphic.program.AddUniformLocation("in_QuadraticAttenuation").Set(0.1f);

            BoundingBox.Max = new Vector3d(1, 1, 1) * Scale;
            BoundingBox.Min = new Vector3d(-1, -1, -1) * Scale;
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
