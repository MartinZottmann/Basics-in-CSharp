using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Graphics
{
    class VertexBufferObject : IBindable, IDisposable
    {
        public uint id;

        public BufferTarget target;

        public VertexBufferObject(BufferTarget target, dynamic[] data)
        {
            this.target = target;
            GL.GenBuffers(1, out id);
            using (new Bind(this))
            {
                GL.BufferData(target, (IntPtr)(data.Length * BlittableValueType.StrideOf(data)), IntPtr.Zero, BufferUsageHint.StaticDraw);
            }
        }

        public void Bind()
        {
            GL.BindBuffer(target, id);
        }

        public void UnBind()
        {
            GL.BindBuffer(target, 0);
        }

        public void Dispose()
        {
            GL.DeleteBuffers(1, ref id);
        }
    }
}
