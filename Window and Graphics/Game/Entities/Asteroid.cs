﻿using MartinZottmann.Engine;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Graphics.Shapes;
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
            graphic.texture = new Texture("res/textures/debug-256.png", false, TextureTarget.Texture2D);

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
            Matrix4d.CreateTranslation(ref Position, out Model);
            ModelUniform.Set(Model);
            ViewUniform.Set(View);
            ModelViewUniform.Set(Model * View);
            //NormalMatrixUniform.Set(Matrix4d.Transpose(Matrix4d.Invert(Model)));
            NormalViewUniform.Set(Matrix4d.Transpose(Matrix4d.Invert(Model * View)));
            ModelViewProjectionUniform.Set(ModelViewProjection);
            graphic.Draw();
        }
    }
}