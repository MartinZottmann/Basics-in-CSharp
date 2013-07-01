using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Engine.Graphics
{
    class SubBuffer<T> where T : struct
    {
        public BufferTarget target;

        public SubBuffer(BufferTarget target, ref int offset, ref T[] data)
        {
            this.target = target;

            var size = data.Length * BlittableValueType.StrideOf(data);
            GL.BufferSubData(target, (IntPtr)offset, (IntPtr)size, data);
            offset += size;
        }
    }
}
