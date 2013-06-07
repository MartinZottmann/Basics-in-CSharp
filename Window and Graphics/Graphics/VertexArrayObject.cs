using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Graphics
{
    class VertexArrayObject : IBindable, IDisposable
    {
        public uint id;

        public VertexArrayObject()
        {
            GL.GenVertexArrays(1, out id);
        }

        public void Add(VertexBufferObject vbo)
        {
            using (new Bind(this))
            {
            }
        }

        public void Bind()
        {
            GL.BindVertexArray(id);
        }

        public void UnBind()
        {
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            GL.DeleteVertexArrays(1, ref id);
        }
    }
}
