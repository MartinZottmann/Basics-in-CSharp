using MartinZottmann.Graphics;
using MartinZottmann.Graphics.OpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Entities
{
    class Asteroid : Physical
    {
        Graphics.OpenGL.Entity graphic;

        public Asteroid()
            : base()
        {
            graphic = new Graphics.OpenGL.Entity();
            graphic.vertex_data = new VertexData[]
            {
                new VertexData(-1.0f, -1.0f,  1.0f, 1.0f, 1.0f, 1.0f, 1.0f),
                new VertexData( 1.0f, -1.0f,  1.0f, 1.0f, 1.0f, 0.0f, 1.0f),
                new VertexData( 1.0f,  1.0f,  1.0f, 1.0f, 0.0f, 1.0f, 1.0f),
                new VertexData(-1.0f,  1.0f,  1.0f, 1.0f, 0.0f, 0.0f, 1.0f),
                new VertexData(-1.0f, -1.0f, -1.0f, 0.0f, 1.0f, 1.0f, 1.0f),
                new VertexData( 1.0f, -1.0f, -1.0f, 0.0f, 1.0f, 0.0f, 1.0f),
                new VertexData( 1.0f,  1.0f, -1.0f, 0.0f, 0.0f, 1.0f, 1.0f),
                new VertexData(-1.0f,  1.0f, -1.0f, 0.0f, 0.0f, 0.0f, 1.0f)
            };
            graphic.elements = new uint[] {
                0, 1, 2, 2, 3, 0,
                3, 2, 6, 6, 7, 3,
                7, 6, 5, 5, 4, 7,
                4, 0, 3, 3, 7, 4,
                0, 1, 5, 5, 4, 0,
                1, 5, 6, 6, 2, 1
            };
            using (var vertex_shader = new Shader(ShaderType.VertexShader, @"
#version 330 core

in  vec3 in_Position;
in  vec4 in_Color;
out vec4 ex_Color;

void main(void) {
    gl_Position = ftransform();
    ex_Color = in_Color;
}
            "))
            using (var fragment_shader = new Shader(ShaderType.FragmentShader, @"
#version 330 core

in  vec4 ex_Color;

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
            graphic.Load();
        }

        public override void Render(double delta_time)
        {
            GL.PushMatrix();
            {
                GL.Rotate(Angle, Vector3d.UnitY);
                GL.Translate(Position.X, Position.Y, Position.Z);

                graphic.Draw();
            }
            GL.PopMatrix();

#if DEBUG
            RenderVelocity(delta_time);
#endif
        }
    }
}
