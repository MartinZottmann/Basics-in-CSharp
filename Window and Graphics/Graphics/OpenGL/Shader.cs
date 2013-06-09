using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Graphics.OpenGL
{
    public class Shader : IDisposable
    {
        public int id;

        public ShaderType type;

        public Shader(ShaderType type, string source)
        {
            id = GL.CreateShader(type);

            GL.ShaderSource(id, source);
            GL.CompileShader(id);
#if DEBUG
            OpenGL.Info.Shader(id);

            int info;
            GL.GetShader(id, ShaderParameter.InfoLogLength, out info);
            if (info != 0)
            {
                string info_log;
                GL.GetShaderInfoLog(id, out info_log);
                Console.WriteLine(info_log);
            }

            GL.GetShader(id, ShaderParameter.CompileStatus, out info);
            if (info != 1)
            {
                throw new Exception("CompileShader failed");
            }
#endif
        }

        public void Dispose()
        {
            GL.DeleteShader(id);
        }
    }
}
