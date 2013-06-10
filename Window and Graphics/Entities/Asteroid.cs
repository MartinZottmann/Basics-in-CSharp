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

            var vertex_data = new VertexP3N3T2SOA(
                // Vertex Data
                new Vector3[] {
                    // Front face
                    new Vector3(-1.0f, -1.0f, 1.0f),
                    new Vector3(1.0f, -1.0f, 1.0f),
                    new Vector3(1.0f, 1.0f, 1.0f),
                    new Vector3(-1.0f, 1.0f, 1.0f),
                    // Right face
                    new Vector3(1.0f, -1.0f, 1.0f),
                    new Vector3(1.0f, -1.0f, -1.0f),
                    new Vector3(1.0f, 1.0f, -1.0f),
                    new Vector3(1.0f, 1.0f, 1.0f),
                    // Back face
                    new Vector3(1.0f, -1.0f, -1.0f),
                    new Vector3(-1.0f, -1.0f, -1.0f),
                    new Vector3(-1.0f, 1.0f, -1.0f),
                    new Vector3(1.0f, 1.0f, -1.0f),
                    // Left face
                    new Vector3(-1.0f, -1.0f, -1.0f),
                    new Vector3(-1.0f, -1.0f, 1.0f),
                    new Vector3(-1.0f, 1.0f, 1.0f),
                    new Vector3(-1.0f, 1.0f, -1.0f),
                    // Top Face
                    new Vector3(-1.0f, 1.0f, 1.0f),
                    new Vector3(1.0f, 1.0f, 1.0f),
                    new Vector3(1.0f, 1.0f, -1.0f),
                    new Vector3(-1.0f, 1.0f, -1.0f),
                    // Bottom Face
                    new Vector3(1.0f, -1.0f, 1.0f),
                    new Vector3(-1.0f, -1.0f, 1.0f),
                    new Vector3(-1.0f, -1.0f, -1.0f),
                    new Vector3(1.0f, -1.0f, -1.0f)
                },
                // Normal Data for the Cube Verticies
                new Vector3[] {
                    // Front face
                    new Vector3(0f, 0f, 1f),
                    new Vector3(0f, 0f, 1f),
                    new Vector3(0f, 0f, 1f),
                    new Vector3(0f, 0f, 1f),
                    // Right face
                    new Vector3(1f, 0f, 0f),
                    new Vector3(1f, 0f, 0f),
                    new Vector3(1f, 0f, 0f),
                    new Vector3(1f, 0f, 0f),
                    // Back face
                    new Vector3(0f, 0f, -1f),
                    new Vector3(0f, 0f, -1f),
                    new Vector3(0f, 0f, -1f),
                    new Vector3(0f, 0f, -1f),
                    // Left face
                    new Vector3(-1f, 0f, 0f),
                    new Vector3(-1f, 0f, 0f),
                    new Vector3(-1f, 0f, 0f),
                    new Vector3(-1f, 0f, 0f),
                    // Top Face
                    new Vector3(0f, 1f, 0f),
                    new Vector3(0f, 1f, 0f),
                    new Vector3(0f, 1f, 0f),
                    new Vector3(0f, 1f, 0f),
                    // Bottom Face
                    new Vector3(0f, -1f, 0f),
                    new Vector3(0f, -1f, 0f),
                    new Vector3(0f, -1f, 0f),
                    new Vector3(0f, -1f, 0f)
                },
                // Texture Data for the Cube Verticies 
                new Vector2[] {
                    // Font Face
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    new Vector2(0, 0),
                    // Right Face
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    new Vector2(0, 0),
                    // Back Face
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    new Vector2(0, 0),
                    // Left Face
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    new Vector2(0, 0),
                    // Top Face
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    new Vector2(0, 0),
                    // Bottom Face
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    new Vector2(0, 0)
                }
            );
            //var vertex_data = new VertexP3N3T2[] {
            //    new VertexP3N3T2(-1.0f, -1.0f,  1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f),
            //    new VertexP3N3T2( 1.0f, -1.0f,  1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f),
            //    new VertexP3N3T2( 1.0f,  1.0f,  1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f),
            //    new VertexP3N3T2(-1.0f,  1.0f,  1.0f, 0.0f, 1.0f, 1.0f, 1.0f, 0.0f),
            //    new VertexP3N3T2(-1.0f, -1.0f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f),
            //    new VertexP3N3T2( 1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f),
            //    new VertexP3N3T2( 1.0f,  1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f),
            //    new VertexP3N3T2(-1.0f,  1.0f, -1.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f)
            //};
            using (new Bind(graphic.vao))
            {
                int p, n, t, size, vertex_attribute = 0;

                GL.GenBuffers(1, out p);
                GL.BindBuffer(BufferTarget.ArrayBuffer, p);
                size = vertex_data.position.Length * BlittableValueType.StrideOf(vertex_data.position);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)size, vertex_data.position, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(vertex_attribute, 3, VertexAttribPointerType.Float, false, BlittableValueType.StrideOf(vertex_data.position), 0);
                GL.EnableVertexAttribArray(vertex_attribute);
                vertex_attribute++;

                GL.GenBuffers(1, out n);
                GL.BindBuffer(BufferTarget.ArrayBuffer, n);
                size = vertex_data.normal.Length * BlittableValueType.StrideOf(vertex_data.normal);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)size, vertex_data.normal, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(vertex_attribute, 3, VertexAttribPointerType.Float, false, BlittableValueType.StrideOf(vertex_data.normal), 0);
                GL.EnableVertexAttribArray(vertex_attribute);
                vertex_attribute++;

                GL.GenBuffers(1, out t);
                GL.BindBuffer(BufferTarget.ArrayBuffer, t);
                size = vertex_data.texcoord.Length * BlittableValueType.StrideOf(vertex_data.texcoord);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)size, vertex_data.texcoord, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(vertex_attribute, 2, VertexAttribPointerType.Float, false, BlittableValueType.StrideOf(vertex_data.texcoord), 0);
                GL.EnableVertexAttribArray(vertex_attribute);
                vertex_attribute++;
            }

            // Element Indices for the Cube
            graphic.elements = new uint[] {
                // Font face
                0, 1, 2, 2, 3, 0,
                // Right face
                7, 6, 5, 5, 4, 7,
                // Back face
                11, 10, 9, 9, 8, 11,
                // Left face
                15, 14, 13, 13, 12, 15,
                // Top Face
                19, 18, 17, 17, 16, 19,
                // Bottom Face
                23, 22, 21, 21, 20, 23,
            };
            using (var vertex_shader = new Shader(ShaderType.VertexShader, @"
#version 330 compatibility

layout(location = 0) in vec3 in_Position;
layout(location = 1) in vec3 in_Normal;
layout(location = 2) in vec2 in_Texcoord;

out vec2 uv;

void main(void)
{
    gl_Position = ftransform();
    uv = in_Texcoord;
}
            "))
            using (var fragment_shader = new Shader(ShaderType.FragmentShader, @"
#version 330 compatibility

uniform sampler2D in_Texture;
in vec2 uv;

void main(void)
{
    gl_FragColor = texture2D(in_Texture, uv);
}
            "))
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
            graphic.Load();
        }

        public override void Render(double delta_time)
        {
            GL.PushMatrix();
            {
                GL.Color3(System.Drawing.Color.White);
                //GL.Rotate(Angle, Vector3d.UnitY);
                GL.Scale(10, 10, 10);
                GL.Translate(Position.X, Position.Y, Position.Z);

                graphic.Draw();
            }
            GL.PopMatrix();

            //#if DEBUG
            //            RenderVelocity(delta_time);
            //#endif
        }
    }
}
