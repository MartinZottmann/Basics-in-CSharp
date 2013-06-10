using MartinZottmann.Graphics;
using MartinZottmann.Graphics.OpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace MartinZottmann.Entities
{
    class Starfield : Entity
    {
        const int num_stars = 100000;

        Graphics.OpenGL.Entity graphic;

        public Starfield()
            : base()
        {
            graphic = new Graphics.OpenGL.Entity();
            using (var vertex_shader = new Shader(ShaderType.VertexShader, @"
#version 330 compatibility

in vec3 in_Position;
in vec4 in_Color;
out vec4 ex_Color;

void main(void) {
    gl_Position = ftransform();
    ex_Color = in_Color;
}
            "))
            using (var fragment_shader = new Shader(ShaderType.FragmentShader, @"
#version 330 compatibility

in vec4 ex_Color;

void main(void) {
    gl_FragColor = ex_Color;
}
            "))
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
            graphic.vertex_data = new VertexData[num_stars];
            for (int i = 0; i < num_stars; i++)
            {
                graphic.vertex_data[i].position.x = randomNumber.Next(-1000, 1000);
                graphic.vertex_data[i].position.y = randomNumber.Next(-1000, 1000);
                graphic.vertex_data[i].position.z = randomNumber.Next(-1000, 1000);
                graphic.vertex_data[i].color.r = (float)randomNumber.NextDouble();
                graphic.vertex_data[i].color.g = (float)randomNumber.NextDouble();
                graphic.vertex_data[i].color.b = (float)randomNumber.NextDouble();
                graphic.vertex_data[i].color.a = (float)randomNumber.NextDouble();
            }
            graphic.Load();
        }

        public override void Render(double delta_time)
        {
            GL.Color3(color);
            GL.PointSize(3);
            graphic.Draw();
        }
    }
}
