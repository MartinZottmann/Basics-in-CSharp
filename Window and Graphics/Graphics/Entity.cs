using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace MartinZottmann.Graphics
{
    public class Entity
    {
        public BeginMode mode = BeginMode.Triangles;

        public Vertex3[] vertices;

        public int[] colors;

        public uint vertex_buffer_object_id;

        public uint vertex_array_object_id;

        public int[] elements;

        public uint element_buffer_object_id;

        public void Load()
        {
            #region Vertex Buffer Object
            GL.GenBuffers(1, out vertex_buffer_object_id);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertex_buffer_object_id);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * BlittableValueType.StrideOf(vertices)), vertices, BufferUsageHint.StaticDraw);
#if DEBUG
            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            System.Diagnostics.Debug.Assert(vertices.Length * BlittableValueType.StrideOf(vertices) == size);
#endif
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
            GL.EnableVertexAttribArray(0);
            //GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, BlittableValueType.StrideOf(vertices), 0);
            //GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, BlittableValueType.StrideOf(vertices), sizeof(float) * 3);
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
            //GL.UseProgram(...);
            GL.BindVertexArray(vertex_array_object_id);

            if (elements == null)
            {
                //GL.EnableClientState(ArrayCap.VertexArray);
                //GL.BindBuffer(BufferTarget.ArrayBuffer, vertex_buffer_object_id);
                GL.DrawArrays(mode, 0, vertices.Length);
            }
            else
            {
                //GL.BindBuffer(BufferTarget.ElementArrayBuffer, element_buffer_object_id);
                GL.DrawElements(mode, elements.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            }

            GL.BindVertexArray(0);
            //GL.UseProgram(0);
        }
    }
}