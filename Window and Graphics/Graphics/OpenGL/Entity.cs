using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace MartinZottmann.Graphics.OpenGL
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
            vao = new VertexArrayObject<VertexData>(vbo);

            if (elements != null)
                eao = new BufferObject<uint>(BufferTarget.ElementArrayBuffer, elements);
        }

        public void Unload()
        {
            if (eao != null)
                eao.Dispose();

            vao.Dispose();

            vbo.Dispose();
        }

        public void Draw()
        {
            using (program == null ? null : new Bind(program))
            using (new Bind(vao))
                if (elements == null)
                    GL.DrawArrays(mode, 0, vertex_data.Length);
                else
                    using (new Bind(eao))
                        GL.DrawElements(mode, elements.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }
    }
}