using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MartinZottmann.Engine.Graphics.OpenGL
{
    public class Program : IBindable, IDisposable
    {
        public int id;

        public IDictionary<string, UniformBlockIndex> UniformBlockIndices = new Dictionary<string, UniformBlockIndex>();

        public IDictionary<string, UniformLocation> UniformLocations = new Dictionary<string, UniformLocation>();

        public Program(Shader[] shaders)
        {
            id = GL.CreateProgram();
            Debug.Assert(id != -1, "CreateProgram failed");

            foreach (var shader in shaders)
                GL.AttachShader(id, shader.id);

            GL.LinkProgram(id);

            using (new Bind(this))
            {
                int count;

                // Setup Uniform Location
                GL.GetProgram(id, ProgramParameter.ActiveUniforms, out count);
                for (int i = 0; i < count; i++)
                {
                    var name = GL.GetActiveUniformName(id, i);
                    UniformLocations[name] = new UniformLocation(this, i, name);
                }

                // Setup Uniform Block Index
                GL.GetProgram(id, ProgramParameter.ActiveUniformBlocks, out count);
                for (int i = 0; i < count; i++)
                {
                    var name = GL.GetActiveUniformBlockName(id, i);
                    UniformBlockIndices[name] = new UniformBlockIndex(this, i, name);
                }
            }

#if DEBUG
            OpenGL.Info.ProgramParameters(id);
            //OpenGL.Info.Uniform(id);

            int info;
            GL.GetProgram(id, ProgramParameter.InfoLogLength, out info);
            if (info > 1)
            {
                string info_log;
                GL.GetProgramInfoLog(id, out info_log);
                throw new Exception(info_log);
            }
            GL.GetProgram(id, ProgramParameter.LinkStatus, out info);
            if (info != 1)
                throw new Exception("LinkProgram failed");
#endif
        }

        public void Bind()
        {
            GL.UseProgram(id);
        }

        public void UnBind()
        {
            GL.UseProgram(0);
        }

        public void Dispose()
        {
            GL.DeleteProgram(id);
        }
    }
}
