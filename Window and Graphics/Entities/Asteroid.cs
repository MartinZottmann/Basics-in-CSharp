using MartinZottmann.Graphics;
using MartinZottmann.Graphics.OpenGL;
using MartinZottmann.Graphics.Shapes;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;

namespace MartinZottmann.Entities
{
    class Asteroid : Physical
    {
        Graphics.OpenGL.Entity graphic;

        UniformLocation ProjectionUniform;

        UniformLocation ModelViewUniform;

        UniformLocation ModelUniform;

        UniformLocation ViewUniform;

        UniformLocation ModelViewProjectionUniform;

        UniformLocation NormalMatrix;

        public UniformLocation EyeDirection;

        public Asteroid()
            : base()
        {
            graphic = new Graphics.OpenGL.Entity();

            //using (new Bind(graphic.vao))
            //{
            //    int p, n, t, size, vertex_attribute = 0;

            //    GL.GenBuffers(1, out p);
            //    GL.BindBuffer(BufferTarget.ArrayBuffer, p);
            //    size = vertex_data.position.Length * BlittableValueType.StrideOf(vertex_data.position);
            //    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)size, vertex_data.position, BufferUsageHint.StaticDraw);
            //    GL.VertexAttribPointer(vertex_attribute, 3, VertexAttribPointerType.Float, false, BlittableValueType.StrideOf(vertex_data.position), 0);
            //    GL.EnableVertexAttribArray(vertex_attribute);
            //    vertex_attribute++;

            //    GL.GenBuffers(1, out n);
            //    GL.BindBuffer(BufferTarget.ArrayBuffer, n);
            //    size = vertex_data.normal.Length * BlittableValueType.StrideOf(vertex_data.normal);
            //    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)size, vertex_data.normal, BufferUsageHint.StaticDraw);
            //    GL.VertexAttribPointer(vertex_attribute, 3, VertexAttribPointerType.Float, false, BlittableValueType.StrideOf(vertex_data.normal), 0);
            //    GL.EnableVertexAttribArray(vertex_attribute);
            //    vertex_attribute++;

            //    GL.GenBuffers(1, out t);
            //    GL.BindBuffer(BufferTarget.ArrayBuffer, t);
            //    size = vertex_data.texcoord.Length * BlittableValueType.StrideOf(vertex_data.texcoord);
            //    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)size, vertex_data.texcoord, BufferUsageHint.StaticDraw);
            //    GL.VertexAttribPointer(vertex_attribute, 2, VertexAttribPointerType.Float, false, BlittableValueType.StrideOf(vertex_data.texcoord), 0);
            //    GL.EnableVertexAttribArray(vertex_attribute);
            //    vertex_attribute++;
            //}

            //// Element Indices for the Cube
            //graphic.elements = new uint[] {
            //    // Font face
            //    0, 1, 2, 2, 3, 0,
            //    // Right face
            //    7, 6, 5, 5, 4, 7,
            //    // Back face
            //    11, 10, 9, 9, 8, 11,
            //    // Left face
            //    15, 14, 13, 13, 12, 15,
            //    // Top Face
            //    19, 18, 17, 17, 16, 19,
            //    // Bottom Face
            //    23, 22, 21, 21, 20, 23,
            //};
            var cube = new Cube();
            var scale = (float)(randomNumber.NextDouble() * 5 + 1);
            for (int i = 0; i < cube.VerticesLength; i++)
            {
                cube.Vertices[i].position *= scale;
            }
            Position.X = (randomNumber.NextDouble() - 0.5) * 25;
            Position.Y = (randomNumber.NextDouble() - 0.5) * 25;
            Position.Z = (randomNumber.NextDouble() - 0.5) * 25;
            graphic.Add(cube);
            var sr = new StreamReader("res/Shaders/point_light.vs.glsl");
            var vs = sr.ReadToEnd();
            sr.Close();
            sr = new StreamReader("res/Shaders/point_light.fs.glsl");
            var fs = sr.ReadToEnd();
            sr.Close();
            using (var vertex_shader = new Shader(ShaderType.VertexShader, vs))
            using (var fragment_shader = new Shader(ShaderType.FragmentShader, fs))
                graphic.program = new Graphics.OpenGL.Program(
                    new Shader[] {
                        vertex_shader,
                        fragment_shader
                    },
                    new string[] {
                        "in_Position",
                        "in_Normal",
                        "in_Texcoord"
                    }
                );
            graphic.texture = new Texture("res/textures/debug-256.png", false, TextureTarget.Texture2D);
            var in_texture = graphic.program.AddUniformLocation("in_Texture");
            in_texture.Set(0);
            //in_texture.Set(graphic.texture.id);

            //ProjectionUniform = graphic.program.AddUniformLocation("in_Projection");
            //ModelViewUniform = graphic.program.AddUniformLocation("in_ModelView");
            ModelViewProjectionUniform = graphic.program.AddUniformLocation("in_ModelViewProjection");
            //graphic.program.AddUniformLocation("in_AmbientColor").Set(new OpenTK.Graphics.Color4(127, 127, 127, 255));
            //graphic.program.AddUniformLocation("in_DiffuseColor").Set(new OpenTK.Graphics.Color4(127, 127, 127, 255));
            //graphic.program.AddUniformLocation("in_SpecularColor").Set(new OpenTK.Graphics.Color4(127, 127, 127, 255));
            graphic.program.AddUniformLocation("in_AmbientLight").Set(new OpenTK.Graphics.Color4(127, 0, 0, 255));
            NormalMatrix = graphic.program.AddUniformLocation("in_NormalMatrix");
            graphic.program.AddUniformLocation("in_LightColor").Set(new OpenTK.Graphics.Color4(127, 127, 0, 255));
            graphic.program.AddUniformLocation("in_LightPosition").Set(new Vector3(1, 1, 1));
            graphic.program.AddUniformLocation("in_Shininess").Set(1f);
            graphic.program.AddUniformLocation("in_Strength").Set(0.1f);
            EyeDirection = graphic.program.AddUniformLocation("in_EyeDirection");
            graphic.program.AddUniformLocation("in_ConstantAttenuation").Set(0.1f);
            graphic.program.AddUniformLocation("in_LinearAttenuation").Set(0.1f);
            graphic.program.AddUniformLocation("in_QuadraticAttenuation").Set(0.1f);
        }

        public override void Render(double delta_time)
        {
            GL.PushMatrix();
            {
                GL.Color3(System.Drawing.Color.White);
                //GL.Rotate(Angle, Vector3d.UnitY);
                GL.Translate(Position.X, Position.Y, Position.Z);

                //ProjectionUniform.Set(Projection);
                Matrix4d.CreateTranslation(ref Position, out Model);
                //ModelViewUniform.Set(Model * View);
                NormalMatrix.Set(Matrix4d.Transpose(Matrix4d.Invert(Model * View)));
                ModelViewProjectionUniform.Set(ModelViewProjection);
                graphic.Draw();
            }
            GL.PopMatrix();

            //#if DEBUG
            //            RenderVelocity(delta_time);
            //#endif
        }
    }
}
