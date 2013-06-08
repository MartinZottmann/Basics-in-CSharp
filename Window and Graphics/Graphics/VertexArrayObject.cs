using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Graphics
{
    public class VertexArrayObject : IBindable, IDisposable
    {
        public uint id;

        public VertexArrayObject(VertexBufferObject vbo)
        {
            using (new Bind(vbo))
            {
                GL.GenVertexArrays(1, out id);
                using (new Bind(this))
                {
                    var size = 0;
                    var vertex_attribute = 0;
                    var offset = 0;
                    if (vbo.vertex_object.vertices != null)
                    {
                        GL.EnableVertexAttribArray(vertex_attribute);
                        size = BlittableValueType.StrideOf(vbo.vertex_object.vertices);
                        GL.VertexAttribPointer(vertex_attribute, 3, VertexAttribPointerType.Float, false, size, offset);
                        vertex_attribute++;
                        offset += vbo.vertex_object.vertices.Length * size;
                    }
                    if (vbo.vertex_object.colors != null)
                    {
                        GL.EnableVertexAttribArray(vertex_attribute);
                        size = BlittableValueType.StrideOf(vbo.vertex_object.colors);
                        GL.VertexAttribPointer(vertex_attribute, 4, VertexAttribPointerType.Float, false, size, offset);
                        vertex_attribute++;
                        offset += vbo.vertex_object.colors.Length * size;
                    }
                }
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
