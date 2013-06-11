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

        public VertexArrayObject vao = new VertexArrayObject();

        public IMesh mesh;

        public Entity() : base() { }

        public void Add<V, I>(Mesh<V, I> mesh)
            where V : struct
            where I : struct
        {
            this.mesh = mesh;
            vao.Add(new BufferObject<V>(BufferTarget.ArrayBuffer, mesh.Vertices));
            vao.Add(new BufferObject<I>(BufferTarget.ElementArrayBuffer, mesh.Indices));
        }

        [Obsolete("Use Add<V, I>(Mesh<V, I> mesh) instead", true)]
        public void Add<U>(BufferObject<U> bo) where U : struct
        {
            vao.Add(bo);
        }

        public void Unload()
        {
            vao.Dispose();
        }

        public void Draw()
        {
            using (texture == null ? null : new Bind(texture))
            using (program == null ? null : new Bind(program))
            using (new Bind(vao))
                if (mesh.IndicesLength == 0)
                    GL.DrawArrays(mode, 0, mesh.VerticesLength);
                else
                    GL.DrawElements(mode, mesh.IndicesLength, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }
    }
}