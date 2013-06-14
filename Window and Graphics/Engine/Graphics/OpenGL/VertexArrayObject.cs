using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Reflection;

namespace MartinZottmann.Engine.Graphics.OpenGL
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
            using (new Bind(this))
            {
                switch (bo.target)
                {
                    case BufferTarget.ArrayBuffer:
                        using (new Bind(bo)) // to VertexAttribPointer
                        {
                            var vertex_attribute = 0;
                            var offset = 0;
                            var stride = BlittableValueType.StrideOf(bo.data);

                            FieldInfo[] fi = bo.data[0].GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                            foreach (FieldInfo info in fi)
                            {
                                int size = 0;
                                VertexAttribPointerType type = VertexAttribPointerType.Byte;
                                foreach (FieldInfo info2 in info.FieldType.GetFields(BindingFlags.Public | BindingFlags.Instance))
                                {
                                    size++;
                                    switch (Type.GetTypeCode(info2.FieldType))
                                    {
                                        case TypeCode.Single: type = VertexAttribPointerType.Float; break;
                                        default: throw new NotImplementedException();
                                    }
                                }

                                GL.VertexAttribPointer(vertex_attribute, size, type, false, stride, offset);
                                GL.EnableVertexAttribArray(vertex_attribute);
                                vertex_attribute++;
                                offset += System.Runtime.InteropServices.Marshal.SizeOf(info.FieldType);
                            }
                        }
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
