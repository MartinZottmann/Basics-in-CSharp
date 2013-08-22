using MartinZottmann.Engine.Graphics.Mesh;
using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Engine.Graphics.OpenGL
{
    [Serializable]
    public class Entity : IDisposable
    {
        public BeginMode Mode = BeginMode.Triangles;

        [NonSerialized]
        public Program Program;

        [NonSerialized]
        public Texture Texture;

        [NonSerialized]
        public readonly VertexArrayObject VertexArrayObject = new VertexArrayObject();

        [NonSerialized]
        protected IMesh mesh;

        public Entity() : base() { }

        public IMesh Mesh() { return mesh; }

        public void Mesh<V, I>(Mesh<V, I> mesh)
            where V : struct, IVertex
            where I : struct
        {
            this.mesh = mesh;
            VertexArrayObject.Add(new BufferObject<V>(BufferTarget.ArrayBuffer, mesh.Vertices));
            if (mesh.IndicesLength != 0)
                VertexArrayObject.Add(new BufferObject<I>(BufferTarget.ElementArrayBuffer, mesh.Indices));
        }

        public void Dispose()
        {
            VertexArrayObject.Dispose();
        }

        public virtual void Draw()
        {
            if (Program != null)
                Program.CheckUniforms();

            using (new Bind(Texture))
            using (new Bind(Program))
            using (new Bind(VertexArrayObject))
                if (mesh.IndicesLength == 0)
                    GL.DrawArrays(Mode, 0, mesh.VerticesLength);
                else
                    GL.DrawElements(Mode, mesh.IndicesLength, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        public virtual void Draw(Program program)
        {
            if (program != null)
                program.CheckUniforms();

            using (new Bind(program))
            using (new Bind(VertexArrayObject))
                if (mesh.IndicesLength == 0)
                    GL.DrawArrays(Mode, 0, mesh.VerticesLength);
                else
                    GL.DrawElements(Mode, mesh.IndicesLength, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }
    }
}
