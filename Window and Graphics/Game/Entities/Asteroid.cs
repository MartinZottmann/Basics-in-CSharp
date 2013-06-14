using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Graphics.Shapes;
using MartinZottmann.Engine.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Game.Entities
{
    class Asteroid : Physical
    {
        Engine.Graphics.OpenGL.Entity graphic;

        UniformLocation ModelUniform;

        UniformLocation ViewUniform;

        UniformLocation ProjectionUniform;

        UniformLocation ModelViewUniform;

        UniformLocation ViewProjectionUniform;

        UniformLocation ModelViewProjectionUniform;

        UniformLocation NormalMatrixUniform;

        UniformLocation NormalViewUniform;

        public UniformLocation EyeDirection;

        public Asteroid(Resources resources)
            : base(resources)
        {
            var cube = new CubeHardNormals();
            var scale = (float)(randomNumber.NextDouble() * 5 + 1);
            for (int i = 0; i < cube.VerticesLength; i++)
                cube.Vertices[i].position *= scale;
            Position.X = (randomNumber.NextDouble() - 0.5) * 25;
            Position.Y = (randomNumber.NextDouble() - 0.5) * 25;
            Position.Z = (randomNumber.NextDouble() - 0.5) * 25;

            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Add(cube);
            graphic.program = Resources.Programs["standard"];
            graphic.texture = Resources.Textures["res/textures/debug-256.png"];

            graphic.program.AddUniformLocation("in_Texture").Set(0);
            //in_texture.Set(graphic.texture.id);

            ModelUniform = graphic.program.AddUniformLocation("in_Model");
            ViewUniform = graphic.program.AddUniformLocation("in_View");
            //ProjectionUniform = graphic.program.AddUniformLocation("in_Projection");
            ModelViewUniform = graphic.program.AddUniformLocation("in_ModelView");
            //ViewProjectionUniform = graphic.program.AddUniformLocation("in_ViewProjection");
            ModelViewProjectionUniform = graphic.program.AddUniformLocation("in_ModelViewProjection");
            //graphic.program.AddUniformLocation("in_AmbientColor").Set(new OpenTK.Graphics.Color4(0, 0, 0, 255));
            //graphic.program.AddUniformLocation("in_DiffuseColor").Set(new OpenTK.Graphics.Color4(255, 255, 255, 255));
            //graphic.program.AddUniformLocation("in_SpecularColor").Set(new OpenTK.Graphics.Color4(127, 127, 127, 255));
            //graphic.program.AddUniformLocation("in_AmbientLight").Set(new OpenTK.Graphics.Color4(127, 127, 127, 255));
            //NormalMatrixUniform = graphic.program.AddUniformLocation("in_NormalMatrix");
            NormalViewUniform = graphic.program.AddUniformLocation("in_NormalView");
            //graphic.program.AddUniformLocation("in_LightColor").Set(new OpenTK.Graphics.Color4(127, 127, 127, 255));
            graphic.program.AddUniformLocation("in_LightPosition").Set(new Vector3(10, 10, 10));
            //graphic.program.AddUniformLocation("in_Shininess").Set(100f);
            //graphic.program.AddUniformLocation("in_Strength").Set(0.1f);
            //EyeDirection = graphic.program.AddUniformLocation("in_EyeDirection");
            //graphic.program.AddUniformLocation("in_ConstantAttenuation").Set(0.1f);
            //graphic.program.AddUniformLocation("in_LinearAttenuation").Set(0.1f);
            //graphic.program.AddUniformLocation("in_QuadraticAttenuation").Set(0.1f);
        }

        public override void Render(double delta_time)
        {
            var model = RenderContext.Model;
            Matrix4d.CreateTranslation(ref Position, out model);
            RenderContext.Model = model;
            ModelUniform.Set(RenderContext.Model);
            ViewUniform.Set(RenderContext.View);
            ModelViewUniform.Set(RenderContext.ViewModel);
            //NormalMatrixUniform.Set(Matrix4d.Transpose(Matrix4d.Invert(Model)));
            NormalViewUniform.Set(RenderContext.Normal);
            ModelViewProjectionUniform.Set(RenderContext.ProjectionViewModel);
            graphic.Draw();
        }
    }
}
