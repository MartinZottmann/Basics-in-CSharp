using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace MartinZottmann.Graphics
{
    public class Entity
    {
        public BeginMode mode = BeginMode.Triangles;

        public Program program;

        public VertexObject vertex_object;

        public Vertex3[] vertices;

        public Color4[] colors;

        // normals, texture_coordinates, lightmap

        public VertexBufferObject vbo;

        public VertexArrayObject vao;

        public BufferObject<uint> eao;

        public uint[] elements;

        public void Load()
        {
            vertex_object = new VertexObject();
            vertex_object.vertices = vertices;
            vertex_object.colors = colors;
            vbo = new VertexBufferObject(BufferTarget.ArrayBuffer, vertex_object);

            if (elements == null)
            {
                vao = new VertexArrayObject(vbo);
            }
            else
            {
                eao = new BufferObject<uint>(BufferTarget.ElementArrayBuffer, elements);
                // @todo vao = new VertexArrayObject(eao);
            }

        }

        public void Unload()
        {
            vao.Dispose();

            if (eao != null)
            {
                eao.Dispose();
            }

            vbo.Dispose();
        }

        public void Draw()
        {
            using (new Bind(program))
            using (new Bind(vao))
                if (elements == null)
                {
                    GL.DrawArrays(mode, 0, vertices.Length);
                }
                else
                {
                    GL.DrawElements(mode, elements.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
                }
        }
    }
}