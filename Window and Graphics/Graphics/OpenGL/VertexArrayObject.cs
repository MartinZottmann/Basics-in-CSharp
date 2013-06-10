using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Reflection;

namespace MartinZottmann.Graphics.OpenGL
{
    public class VertexArrayObject : IBindable, IDisposable
    {
        public uint id;

        public VertexArrayObject()
        {
            GL.GenVertexArrays(1, out id);
        }

        public void Add<U>(BufferObject<U> bo) where U : struct
        {
            //FieldInfo[] fi = bo.data[0].GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            //foreach (FieldInfo info in fi)
            //{
            //    Console.WriteLine(info);
            //}
            using (new Bind(this))
            {
                switch (bo.target)
                {
                    case BufferTarget.ArrayBuffer:
                        if (bo.data is VertexData[])
                        {
                            var vertex_attribute = 0;
                            var offset = 0;
                            var stride = BlittableValueType.StrideOf(bo.data);

                            using (new Bind(bo)) // to VertexAttribPointer
                            {
                                // @todo get vertex_attibute from program/shader via GL.GetAttribLocation(program.id, ...)
                                GL.VertexAttribPointer(vertex_attribute, 3, VertexAttribPointerType.Float, false, stride, offset);
                                GL.EnableVertexAttribArray(vertex_attribute);
                                vertex_attribute++;
                                offset += sizeof(float) * 3;

                                GL.VertexAttribPointer(vertex_attribute, 4, VertexAttribPointerType.Float, false, stride, offset);
                                GL.EnableVertexAttribArray(vertex_attribute);
                                vertex_attribute++;
                                offset += sizeof(float) * 4;
                            }
                        }
                        else if (bo.data is VertexP3N3T2[])
                        {
                            var vertex_attribute = 0;
                            var offset = 0;
                            var stride = BlittableValueType.StrideOf(bo.data);

                            using (new Bind(bo)) // to VertexAttribPointer
                            {
                                // @todo get vertex_attibute from program/shader via GL.GetAttribLocation(program.id, ...)
                                GL.VertexAttribPointer(vertex_attribute, 3, VertexAttribPointerType.Float, false, stride, offset);
                                GL.EnableVertexAttribArray(vertex_attribute);
                                vertex_attribute++;
                                offset += sizeof(float) * 3;

                                GL.VertexAttribPointer(vertex_attribute, 3, VertexAttribPointerType.Float, false, stride, offset);
                                GL.EnableVertexAttribArray(vertex_attribute);
                                vertex_attribute++;
                                offset += sizeof(float) * 3;

                                GL.VertexAttribPointer(vertex_attribute, 2, VertexAttribPointerType.Float, false, stride, offset);
                                GL.EnableVertexAttribArray(vertex_attribute);
                                vertex_attribute++;
                                offset += sizeof(float) * 2;
                            }
                        }
                        else
                            throw new NotImplementedException();
                        break;
                    case BufferTarget.ElementArrayBuffer:
                        bo.Bind(); // Unbinding will result in an address violation
                        //using (new Bind(bo)) { }
                        break;
                    default:
                        throw new NotImplementedException();
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
