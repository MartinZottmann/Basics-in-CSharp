﻿using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MartinZottmann.Engine.Graphics.OpenGL
{
    public class Program : IBindable, IDisposable
    {
        public uint Id;

        public IDictionary<string, UniformBlockIndex> UniformBlockIndices = new Dictionary<string, UniformBlockIndex>();

        public IDictionary<string, UniformLocation> UniformLocations = new Dictionary<string, UniformLocation>();

        public Program(Shader[] shaders)
        {
            var id = GL.CreateProgram();
            Debug.Assert(id != -1, "CreateProgram failed");

            Id = (uint)id;

            foreach (var shader in shaders)
                GL.AttachShader(Id, shader.Id);

            GL.LinkProgram(Id);

            using (new Bind(this))
            {
                int count;

                // Setup Uniform Location
                GL.GetProgram(Id, ProgramParameter.ActiveUniforms, out count);
                for (uint i = 0; i < count; i++)
                {
                    var name = GL.GetActiveUniformName((int)Id, (int)i);
                    UniformLocations[name] = new UniformLocation(this, i, name);
                }

                // Setup Uniform Block Index
                GL.GetProgram(Id, ProgramParameter.ActiveUniformBlocks, out count);
                for (uint i = 0; i < count; i++)
                {
                    var name = GL.GetActiveUniformBlockName((int)Id, (int)i);
                    UniformBlockIndices[name] = new UniformBlockIndex(this, i, name);
                }
            }

#if DEBUG
            //OpenGL.Info.ProgramParameters(Id);
            //OpenGL.Info.Uniform(id);

            int info;
            GL.GetProgram(Id, ProgramParameter.InfoLogLength, out info);
            if (info > 1)
            {
                string info_log;
                GL.GetProgramInfoLog((int)Id, out info_log);
                Console.WriteLine(info_log);
            }
            GL.GetProgram(Id, ProgramParameter.LinkStatus, out info);
            if (info != 1)
                throw new Exception("LinkProgram failed");
#endif
        }

        public void Bind()
        {
            GL.UseProgram(Id);
        }

        public void UnBind()
        {
            GL.UseProgram(0);
        }

        public void Dispose()
        {
            GL.DeleteProgram(Id);
        }
    }
}