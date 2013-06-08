using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace MartinZottmann.Graphics
{
    public class Program : IBindable, IDisposable
    {
        public int id;

        public Program(Shader[] shaders, string[] attribute_location_names = null)
        {
            id = GL.CreateProgram();
            Debug.Assert(id != -1, "CreateProgram failed");

            foreach (var shader in shaders)
            {
                GL.AttachShader(id, shader.id);
            }

            if (attribute_location_names != null)
            {
                for (int i = 0; i < attribute_location_names.Length; i++)
                {
                    GL.BindAttribLocation(id, i, attribute_location_names[i]);
                }
            }

            GL.LinkProgram(id);
#if DEBUG
            OpenGL.Info.ProgramParameters(id);

            int info;
            GL.GetProgram(id, ProgramParameter.InfoLogLength, out info);
            if (info != 0)
            {
                string info_log;
                GL.GetProgramInfoLog(id, out info_log);
                throw new Exception(info_log);
            }
            GL.GetProgram(id, ProgramParameter.LinkStatus, out info);
            if (info != 1)
            {
                throw new Exception("LinkProgram failed");
            }
#endif
        }

        public UniformBlockIndex AddUniformBlockIndex(string name)
        {
            using (new Bind(this))
                return new UniformBlockIndex(this, name);
        }

        public UniformLocation AddUniformLocation(string name)
        {
            using (new Bind(this))
                return new UniformLocation(this, name);
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

        public void Info()
        {
            int shaders, count, info;

            if (!GL.IsProgram(id))
            {
                Console.WriteLine("{0} is not a Program", id);
            }
            Console.WriteLine("Program Information for name {0}", id);

            OpenGL.Info.ProgramParameters(id);

            Console.WriteLine("ActiveUniforms {");
            GL.GetProgram(id, ProgramParameter.ActiveUniforms, out count);
            for (int i = 0; i < count; i++)
            {
                int index;
                GL.GetActiveUniforms(id, 1, ref i, ActiveUniformParameter.UniformBlockIndex, out index);
                //if (index == -1)
                //{
                string name = GL.GetActiveUniformName(id, i);
                foreach (ActiveUniformParameter parameter in (ActiveUniformParameter[])Enum.GetValues(typeof(ActiveUniformParameter)))
                {
                    GL.GetActiveUniforms(id, 1, ref i, parameter, out info);
                    Console.WriteLine("\t{0}: {1}: {2} = {3}", i, name, parameter, info);
                }
                //}
            }
            Console.WriteLine("}");

            Console.WriteLine("ActiveUniformBlocks {");
            GL.GetProgram(id, ProgramParameter.ActiveUniformBlocks, out count);
            for (int i = 0; i < count; i++)
            {
                string name = GL.GetActiveUniformBlockName(id, i);
                foreach (ActiveUniformBlockParameter parameter in (ActiveUniformBlockParameter[])Enum.GetValues(typeof(ActiveUniformBlockParameter)))
                {
                    GL.GetActiveUniformBlock(id, i, parameter, out info);
                    Console.WriteLine("\t{0}: {1}: {2} = {3}", i, name, parameter, info);
                }
            }
            Console.WriteLine("}");

            Console.WriteLine("Shaders {");
            GL.GetProgram(id, ProgramParameter.AttachedShaders, out count);
            Console.WriteLine(count);
            GL.GetAttachedShaders(id, count, out count, out shaders);
            Console.WriteLine(count);
            Console.WriteLine(shaders);
            for (int i = 0; i < count; i++)
            {
                GL.GetShader(shaders + i, ShaderParameter.ShaderType, out info);
                Console.WriteLine("\t{0}: {1}", shaders + i, (ShaderType)info);
            }
            Console.WriteLine("}");
        }
    }
}
