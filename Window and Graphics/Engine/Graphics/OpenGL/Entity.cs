using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Engine.Graphics.OpenGL
{
    public class Entity : IDisposable
    {
        public BeginMode mode = BeginMode.Triangles;

        public Program Program;

        public Texture Texture;

        protected VertexArrayObject vao = new VertexArrayObject();

        protected IMesh mesh;

        public Entity() : base() { }

        public void Add<V, I>(Mesh<V, I> mesh)
            where V : struct
            where I : struct
        {
            this.mesh = mesh;
            vao.Add(new BufferObject<V>(BufferTarget.ArrayBuffer, mesh.Vertices));
            if (mesh.IndicesLength != 0)
                vao.Add(new BufferObject<I>(BufferTarget.ElementArrayBuffer, mesh.Indices));
        }

        public void Dispose()
        {
            vao.Dispose();
        }

        public void Draw()
        {
            using (Texture == null ? null : new Bind(Texture))
            using (Program == null ? null : new Bind(Program))
            using (new Bind(vao))
                if (mesh.IndicesLength == 0)
                    GL.DrawArrays(mode, 0, mesh.VerticesLength);
                else
                    GL.DrawElements(mode, mesh.IndicesLength, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }
    }
}