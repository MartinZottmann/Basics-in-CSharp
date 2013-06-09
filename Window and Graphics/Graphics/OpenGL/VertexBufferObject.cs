using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Graphics.OpenGL
{
    public class VertexBufferObject : IBindable, IDisposable
    {
        public uint id;

        public BufferTarget target;

        public VertexObject vertex_object;

        public VertexBufferObject(BufferTarget target, VertexObject vertex_object, BufferUsageHint usage_hint = BufferUsageHint.StaticDraw)
        {
            this.target = target;
            this.vertex_object = vertex_object;

            GL.GenBuffers(1, out id);
            using (new Bind(this))
            {
                var vertices_size = vertex_object.vertices.Length * BlittableValueType.StrideOf(vertex_object.vertices);
                var colors_size = vertex_object.colors.Length * BlittableValueType.StrideOf(vertex_object.colors);
                var size = vertices_size + colors_size;
                GL.BufferData(target, (IntPtr)size, (IntPtr)null, BufferUsageHint.StaticDraw);
                var offset = 0;
                new SubBuffer<Vertex3>(target, ref offset, ref vertex_object.vertices);
                new SubBuffer<Color4>(target, ref offset, ref vertex_object.colors);
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
