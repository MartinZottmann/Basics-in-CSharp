using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Graphics.OpenGL
{
    public abstract class VertexArrayObject : IBindable, IDisposable
    {
        public abstract void Bind();

        public abstract void UnBind();

        public abstract void Dispose();
    }

    public class VertexArrayObject<T> : VertexArrayObject, IBindable, IDisposable where T : struct
    {
        public uint id;

        public VertexArrayObject(BufferObject<T> vbo)
        {
            using (new Bind(vbo))
            {
                GL.GenVertexArrays(1, out id);
                using (new Bind(this))
                {
                    var vertex_attribute = 0;
                    var offset = 0;
                    var stride = BlittableValueType.StrideOf(vbo.data);

                    GL.EnableVertexAttribArray(vertex_attribute);
                    GL.VertexAttribPointer(vertex_attribute, 3, VertexAttribPointerType.Float, false, stride, offset);
                    vertex_attribute++;
                    offset += sizeof(float) * 3;

                    GL.EnableVertexAttribArray(vertex_attribute);
                    GL.VertexAttribPointer(vertex_attribute, 4, VertexAttribPointerType.Float, false, stride, offset);
                    vertex_attribute++;
                    offset += sizeof(float) * 4;
                }
            }
        }

        public override void Bind()
        {
            GL.BindVertexArray(id);
        }

        public override void UnBind()
        {
            GL.BindVertexArray(0);
        }

        public override void Dispose()
        {
            GL.DeleteVertexArrays(1, ref id);
        }
    }
}
