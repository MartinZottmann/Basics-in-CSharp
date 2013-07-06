using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.Reflection;

namespace MartinZottmann.Engine.Graphics.OpenGL
{
    public abstract class BufferObject : IBindable, IDisposable
    {
        public readonly uint Id;

        public readonly BufferTarget Target;

        public readonly int Stride;

        public readonly int Size;

        public BufferObject(BufferTarget target, int stride, int size)
        {
            Id = (uint)GL.GenBuffer();
            Target = target;
            Stride = stride;
            Size = size;
        }

        public abstract void Bind();

        public abstract void UnBind();

        public abstract void Dispose();
    }

    public class BufferObject<T> : BufferObject, IBindable, IDisposable where T : struct
    {
        public readonly FieldInfo[] DataFieldInfo;

        public BufferObject(BufferTarget target, T[] data, BufferUsageHint usage_hint = BufferUsageHint.StaticDraw)
            : base(target, BlittableValueType.StrideOf(data), data.Length * BlittableValueType.StrideOf(data))
        {
            if (data.Length != 0)
                DataFieldInfo = data[0].GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            using (new Bind(this))
            {
                GL.BufferData(target, (IntPtr)Size, data, usage_hint);
#if DEBUG
                int info;
                GL.GetBufferParameter(target, BufferParameterName.BufferSize, out info);
                Debug.Assert(Size == info);
#endif
            }
        }

        public void Write(int offset, int size, T[] data)
        {
            using (new Bind(this))
                GL.BufferSubData(Target, (IntPtr)offset, (IntPtr)size, data);
        }

        public void Write(int offset, T data)
        {
            using (new Bind(this))
                GL.BufferSubData(Target, (IntPtr)(offset * Stride), (IntPtr)Stride, new T[] { data });
        }

        public void Write(T[] data)
        {
            using (new Bind(this))
                GL.BufferSubData(Target, (IntPtr)0, (IntPtr)Size, data);
        }

        public override void Bind()
        {
            GL.BindBuffer(Target, Id);
        }

        public override void UnBind()
        {
            GL.BindBuffer(Target, 0);
        }

        public override void Dispose()
        {
            GL.DeleteBuffer(Id);
        }
    }
}
