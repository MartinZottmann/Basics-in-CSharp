using MartinZottmann.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace MartinZottmann.Entities
{
    class Starfield : Entity
    {
        const int num_stars = 100000;

        Graphics.Entity graphic;

        public Starfield()
            : base()
        {
            graphic = new Graphics.Entity();
            graphic.program = new Graphics.Program();
            using (var shader = new Shader(ShaderType.VertexShader, @"
#version 150
in  vec3 in_Position;
in  vec4 in_Color;
out vec4 ex_Color;

void main(void) {
    gl_Position = vec4(in_Position, 1.0);
    ex_Color = in_Color;
}
            "))
            {
                graphic.program.AttachShader(shader);
                graphic.program.BindAttribLocation(0, "in_Position");
                graphic.program.BindAttribLocation(1, "in_Color");
            }
            using (var shader = new Shader(ShaderType.FragmentShader, @"
#version 150
precision highp float;

in  vec4 ex_Color;
out vec4 gl_FragColor;

void main(void) {
    gl_FragColor = ex_Color;
}
            "))
            {
                graphic.program.AttachShader(shader);
            }
            graphic.program.Link();
            graphic.mode = BeginMode.Points;
            graphic.vertices = new Graphics.Vertex3[num_stars];
            graphic.colors = new MartinZottmann.Graphics.Color4[num_stars];
            for (int i = 0; i < num_stars; i++)
            {
                graphic.vertices[i].x = randomNumber.Next(-1000, 1000);
                graphic.vertices[i].y = randomNumber.Next(-1000, 1000);
                graphic.vertices[i].z = randomNumber.Next(-1000, 1000);
                graphic.colors[i].r = (float)randomNumber.NextDouble();
                graphic.colors[i].g = (float)randomNumber.NextDouble();
                graphic.colors[i].b = (float)randomNumber.NextDouble();
                graphic.colors[i].a = (float)randomNumber.NextDouble();
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
