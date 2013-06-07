using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Graphics
{
    public class Program : IDisposable
    {
        public int id;

        public Program()
        {
            id = GL.CreateProgram();
        }

        public void AttachShader(int shader)
        {
            GL.AttachShader(id, shader);
        }

        public void AttachShader(Shader shader)
        {
            GL.AttachShader(id, shader.id);
        }

        public void BindAttribLocation(int index, string name)
        {
            GL.BindAttribLocation(id, index, name);
        }

        public void Link()
        {
            GL.LinkProgram(id);
#if DEBUG
            string info_log;
            GL.GetProgramInfoLog(id, out info_log);
            Console.Error.WriteLine(info_log);
#endif

            // we could delete the shaders here
        }

        public void Use()
        {
            GL.UseProgram(id);
        }

        public void Unuse()
        {
            GL.UseProgram(0);
        }

        public void Dispose()
        {
            GL.DeleteProgram(id);
        }
    }
}
