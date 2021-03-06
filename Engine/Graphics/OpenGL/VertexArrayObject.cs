﻿using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MartinZottmann.Engine.Graphics.OpenGL
{
    public class VertexArrayObject : IBindable, IDisposable
    {
        public uint Id { get; protected set; }

        public List<BufferObject> BufferObjects = new List<BufferObject>();

        public VertexArrayObject()
        {
            Id = (uint)GL.GenVertexArray();
        }

        ~VertexArrayObject()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (0 != Id)
                {
                    GL.DeleteVertexArray(Id);
                    Id = 0;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Add<U>(BufferObject<U> bo) where U : struct
        {
            BufferObjects.Add(bo);

            using (new Bind(this))
                switch (bo.Target)
                {
                    case BufferTarget.ArrayBuffer:
                        using (new Bind(bo)) // to VertexAttribPointer
                        {
                            var vertex_attribute = 0;
                            var offset = 0;
                            var stride = bo.Stride;

                            foreach (FieldInfo info in bo.DataFieldInfo)
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

        public void Bind()
        {
            GL.BindVertexArray(Id);
        }

        public void UnBind()
        {
            GL.BindVertexArray(0);
        }
    }
}
