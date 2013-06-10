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

        public Texture texture;

        // public object[] vertex_data;

        // normals, texture_coordinates, lightmap

        //public BufferObject vbo;

        public VertexArrayObject vao = new VertexArrayObject();

        public BufferObject<uint> eao;

        public uint[] elements;

        public Entity()
            : base()
        {
        }

        public void Add<U>(BufferObject<U> bo) where U : struct
        {
            vao.Add(bo);
        }

        public void Load()
        {
            //vbo = new BufferObject<VertexData>(BufferTarget.ArrayBuffer, vertex_data);
            //vao.Add(vbo);
            //vbo = new VertexBufferObject(BufferTarget.ArrayBuffer, vertex_data);

            if (elements != null)
            {
                eao = new BufferObject<uint>(BufferTarget.ElementArrayBuffer, elements);
                Add(eao);
                vao.Add(eao);
            }
        }

        public void Unload()
        {
            if (eao != null)
                eao.Dispose();

            vao.Dispose();

            //vbo.Dispose();
        }

        public void Draw()
        {
            using (texture == null ? null : new Bind(texture))
            using (program == null ? null : new Bind(program))
            using (new Bind(vao))
                if (elements == null)
                    GL.DrawArrays(mode, 0, 10000/*vertex_data.Length*/);
                else
                    GL.DrawElements(mode, elements.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }
    }
}