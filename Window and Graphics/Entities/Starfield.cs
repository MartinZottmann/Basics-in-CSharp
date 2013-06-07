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
            using (var vertex_shader = new Shader(ShaderType.VertexShader, @"
#version 150
precision highp float;

in  vec3 in_Position;
in  vec4 in_Color;
out vec4 ex_Color;

void main(void) {
    gl_Position = ftransform();
    ex_Color = in_Color;
}
            "))
            using (var fragment_shader = new Shader(ShaderType.FragmentShader, @"
#version 150
precision highp float;

in  vec4 ex_Color;

//uniform float delta_time;

void main(void) {
    //gl_FragColor = vec4(ex_Color.r, ex_Color.g, ex_Color.b, delta_time);
    gl_FragColor = ex_Color;
}
            "))
                graphic.program = new Graphics.Program(
                    new Shader[] {
                        vertex_shader,
                        fragment_shader
                    },
                    new string[] {
                        "in_Position",
                        "in_Color"
                    //},
                    //new string[] {
                    //    "delta_time"
                    }
                );
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

        //public override void Update(double delta_time)
        //{
        //    using (new Bind(graphic.program))
        //        graphic.program.uniform_location[0].Set((float)randomNumber.NextDouble());
        //}

        public override void Render(double delta_time)
        {
            GL.Color3(color);
            GL.PointSize(3);
            graphic.Draw();
        }
    }
}
