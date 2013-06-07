using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Graphics
{
    public class Program : IBindable, IDisposable
    {
        public int id;

        public UniformLocation[] uniform_location;

        public Program(Shader[] shaders, string[] attribute_location_names = null, string[] uniform_location_names = null)
        {
            id = GL.CreateProgram();

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

            if (uniform_location_names != null)
            {
                var n = uniform_location_names.Length;
                uniform_location = new UniformLocation[n];
                for (int i = 0; i < n; i++)
                {
                    uniform_location[i] = new UniformLocation(this, uniform_location_names[i]);
                }
            }
#if DEBUG
            string info_log;
            GL.GetProgramInfoLog(id, out info_log);
            Console.Error.WriteLine(info_log);
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
