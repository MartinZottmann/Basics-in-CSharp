using OpenTK.Graphics.OpenGL;
using System;
using System.IO;

namespace MartinZottmann.Engine.Graphics.OpenGL
{
    public class Shader : IDisposable
    {
        public uint Id { get; protected set; }

        public readonly ShaderType Type;

        public Shader(ShaderType type, string filename)
        {
            var sr = new StreamReader(filename);
            var source = sr.ReadToEnd();
            sr.Close();

            Id = (uint)GL.CreateShader(type);
            Type = type;

            GL.ShaderSource((int)Id, source); // GL.ShaderSource(uint shader, string @string) is missing
            try { GL.CompileShader(Id); }
            catch (Exception) { }
#if DEBUG
            //OpenGL.Info.Shader(Id);

            int info;
            GL.GetShader(Id, ShaderParameter.InfoLogLength, out info);
            if (info != 0)
            {
                string info_log;
                GL.GetShaderInfoLog((int)Id, out info_log); // GL.GetShaderInfoLog(uint shader, out string info) is missing
                Console.WriteLine(info_log);
            }

            GL.GetShader(Id, ShaderParameter.CompileStatus, out info);
            if (info != 1)
                throw new Exception("CompileShader failed");
#endif
        }

        ~Shader()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (0 != Id)
                {
                    GL.DeleteShader(Id);
                    Id = 0;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
