﻿using MartinZottmann.Engine;
using MartinZottmann.Graphics;
using MartinZottmann.Graphics.OpenGL;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Entities
{
    class Starfield : Entity
    {
        const int num_stars = 100000;

        Graphics.OpenGL.Entity graphic;

        public Starfield(Resources resources) : base(resources)
        {
            graphic = new Graphics.OpenGL.Entity();
            using (var vertex_shader = new Shader(ShaderType.VertexShader, "res/Shaders/normal.vs.glsl"))
            using (var fragment_shader = new Shader(ShaderType.FragmentShader, "res/Shaders/normal.fs.glsl"))
                graphic.program = new Graphics.OpenGL.Program(
                    new Shader[] {
                        vertex_shader,
                        fragment_shader
                    },
                    new string[] {
                        "in_Position",
                        "in_Color"
                    }
                );
            graphic.mode = BeginMode.Points;
            var vertex_data = new VertexP3C4[num_stars];
            for (int i = 0; i < num_stars; i++)
            {
                vertex_data[i].position.X = randomNumber.Next(-1000, 1000);
                vertex_data[i].position.Y = randomNumber.Next(-1000, 1000);
                vertex_data[i].position.Z = randomNumber.Next(-1000, 1000);
                vertex_data[i].color.r = (float)randomNumber.NextDouble();
                vertex_data[i].color.g = (float)randomNumber.NextDouble();
                vertex_data[i].color.b = (float)randomNumber.NextDouble();
                vertex_data[i].color.a = (float)randomNumber.NextDouble();
            }
            graphic.Add(new Mesh<VertexP3C4, uint>(vertex_data));
            //graphic.Add(new BufferObject<VertexP3C4>(BufferTarget.ArrayBuffer, vertex_data));
        }

        public override void Render(double delta_time)
        {
            GL.Color3(color);
            GL.PointSize(1);
            graphic.Draw();
        }
    }
}
