using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace MartinZottmann.Engine.Graphics.OpenGL
{
    public abstract class BufferObject : IBindable, IDisposable
    {
        public uint id;

        public BufferTarget target;

        public int stride;

        public int size;

        public abstract void Bind();

        public abstract void UnBind();

        public abstract void Dispose();
    }

    public class BufferObject<T> : BufferObject, IBindable, IDisposable where T : struct
    {
        public T[] data;

        public BufferObject(BufferTarget target, T[] data, BufferUsageHint usage_hint = BufferUsageHint.StaticDraw)
        {
            this.target = target;
            this.data = data;
            stride = BlittableValueType.StrideOf(data);
            size = data.Length * stride;

            GL.GenBuffers(1, out id);
            using (new Bind(this))
            {
                GL.BufferData(target, (IntPtr)size, data, usage_hint);
#if DEBUG
                int info;
                GL.GetBufferParameter(target, BufferParameterName.BufferSize, out info);
                Debug.Assert(size == info);
#endif
            }
        }

        public override void Bind()
        {
            GL.BindBuffer(target, id);
        }

        public override void UnBind()
        {
            GL.BindBuffer(target, 0);
        }

        public override void Dispose()
        {
            GL.DeleteBuffers(1, ref id);
        }
    }
}
