using MartinZottmann.Engine.Graphics.Mesh;
using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Engine.Graphics.OpenGL
{
    public class Model : IDisposable
    {
        public PrimitiveType Mode { get; set; }

        public Program Program { get; set; }

        public Texture Texture { get; set; }

        public VertexArrayObject VertexArrayObject { get; protected set; }

        protected IMesh mesh;

        public Model()
        {
            Mode = PrimitiveType.Triangles;
            VertexArrayObject = new VertexArrayObject();
        }

        ~Model()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != VertexArrayObject)
                {
                    VertexArrayObject.Dispose();
                    VertexArrayObject = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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

        public virtual void Draw()
        {
            if (null != Program)
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
            if (null != program)
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
