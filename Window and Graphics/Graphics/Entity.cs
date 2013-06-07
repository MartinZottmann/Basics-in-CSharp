using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace MartinZottmann.Graphics
{
    public class Entity
    {
        public BeginMode mode = BeginMode.Triangles;

        public Program program;

        public Vertex3[] vertices;

        public Color4[] colors;

        // normals, texture_coordinates, lightmap

        public uint vertex_buffer_object_id;

        public uint vertex_array_object_id;

        public uint[] elements;

        public uint element_buffer_object_id;

        public void Load()
        {
#if DEBUG
            //System.Diagnostics.Debug.Assert(vertices.Length == colors.Length);
            int size;
#endif
            #region Vertex Buffer Object
            GL.GenBuffers(1, out vertex_buffer_object_id);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertex_buffer_object_id);
            var vertices_size = vertices == null ? 0 : vertices.Length * BlittableValueType.StrideOf(vertices);
            var colors_size = colors == null ? 0 : colors.Length * BlittableValueType.StrideOf(colors);
            var offset = 0;

            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices_size + colors_size), IntPtr.Zero, BufferUsageHint.StaticDraw);
#if DEBUG
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            System.Diagnostics.Debug.Assert(vertices_size + colors_size == size);
#endif
            if (vertices != null)
            {
                GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)offset, (IntPtr)vertices_size, vertices);
                offset += vertices_size;
            }
            if (colors != null)
            {
                GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)offset, (IntPtr)colors_size, colors);
                offset += colors_size;
            }
            #endregion

            #region Element Buffer Object
            if (elements != null)
            {
                GL.GenBuffers(1, out element_buffer_object_id);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, element_buffer_object_id);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(elements.Length * BlittableValueType.StrideOf(elements)), elements, BufferUsageHint.StaticDraw);
            }
            #endregion

            #region Vertex Array Object
            GL.GenVertexArrays(1, out vertex_array_object_id);
            GL.BindVertexArray(vertex_array_object_id);
            var vertex_attribute = 0;
            offset = 0;
            if (vertices != null)
            {
                GL.EnableVertexAttribArray(vertex_attribute);
                GL.VertexAttribPointer(vertex_attribute, 3, VertexAttribPointerType.Float, false, BlittableValueType.StrideOf(vertices), offset);
                vertex_attribute++;
                offset += vertices_size;
            }
            if (colors != null)
            {
                GL.EnableVertexAttribArray(vertex_attribute);
                GL.VertexAttribPointer(vertex_attribute, 4, VertexAttribPointerType.Float, false, BlittableValueType.StrideOf(colors), offset);
                vertex_attribute++;
                offset += colors_size;
            }
            GL.BindVertexArray(0);
            #endregion
        }

        public void Unload()
        {
            #region Vertex Array Object
            GL.DeleteVertexArrays(1, ref vertex_array_object_id);
            #endregion

            #region Element Buffer Object
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffers(1, ref element_buffer_object_id);
            #endregion

            #region Vertex Buffer Object
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffers(1, ref vertex_buffer_object_id);
            #endregion
        }

        public void Draw()
        {
            if (program != null)
            {
                program.Push();
            }
            GL.BindVertexArray(vertex_array_object_id);

            if (elements == null)
            {
                GL.DrawArrays(mode, 0, vertices.Length);
            }
            else
            {
                GL.DrawElements(mode, elements.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            }

            GL.BindVertexArray(0);
            if (program != null)
            {
                program.Pop();
            }
        }
    }
}