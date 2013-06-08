using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Graphics
{
    public class BufferObject<T> : IBindable, IDisposable where T : struct
    {
        public uint id;

        public BufferTarget target;

        public BufferObject(BufferTarget target, T[] data, BufferUsageHint usage_hint = BufferUsageHint.StaticDraw)
        {
            this.target = target;

            GL.GenBuffers(1, out id);
            using (new Bind(this))
            {
                GL.BufferData(target, (IntPtr)(data.Length * BlittableValueType.StrideOf(data)), data, usage_hint);
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
