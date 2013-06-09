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
            graphic.texture = new Texture("res/textures/debug-256.png", false, TextureTarget.Texture2D);
            using (var vertex_shader = new Shader(ShaderType.VertexShader, @"
#version 330 core

//in vec3 in_Position;
//in vec4 in_Color;

void main(void)
{
    gl_Position = ftransform();
}
            "))
            using (var fragment_shader = new Shader(ShaderType.FragmentShader, @"
#version 330 core

uniform sampler2D in_Texture;

void main(void)
{
    gl_FragColor = texture(in_Texture, gl_TexCoord[0].st);
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
            var in_texture = graphic.program.AddUniformLocation("in_Texture");
            in_texture.Set(0);
            //in_texture.Set(graphic.texture.id);
            graphic.Load();
        }

        public override void Render(double delta_time)
        {
            GL.PushMatrix();
            {
                GL.Color3(color);
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
