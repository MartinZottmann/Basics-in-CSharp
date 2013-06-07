using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Graphics
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
            string info_log;
            GL.GetShaderInfoLog(id, out info_log);
            Console.WriteLine(info_log);

            int compile_result;
            GL.GetShader(id, ShaderParameter.CompileStatus, out compile_result);
            if (compile_result != 1)
            {
                Console.Error.WriteLine("Compile error: {0}", compile_result);
            }
#endif
        }

        public void Dispose()
        {
            GL.DeleteShader(id);
        }
    }
}
