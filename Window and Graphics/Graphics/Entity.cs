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

        public VertexData[] vertex_data;

        // normals, texture_coordinates, lightmap

        public BufferObject<VertexData> vbo;

        public VertexArrayObject vao;

        public BufferObject<uint> eao;

        public uint[] elements;

        public void Load()
        {
            vbo = new BufferObject<VertexData>(BufferTarget.ArrayBuffer, vertex_data);
            //vbo = new VertexBufferObject(BufferTarget.ArrayBuffer, vertex_data);

            if (elements == null)
            {
                vao = new VertexArrayObject<VertexData>(vbo);
            }
            else
            {
                eao = new BufferObject<uint>(BufferTarget.ElementArrayBuffer, elements);
                vao = new VertexArrayObject<uint>(eao);
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
                    GL.DrawArrays(mode, 0, vertex_data.Length);
                }
                else
                {
                    GL.DrawElements(mode, elements.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
                }
        }
    }
}