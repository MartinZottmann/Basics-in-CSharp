﻿using OpenTK.Graphics.OpenGL;
using System;
using System.IO;

namespace MartinZottmann.Engine.Graphics.OpenGL
{
    public class Shader : IDisposable
    {
        public int id;

        public ShaderType type;

        public Shader(ShaderType type, string filename)
        {
            var sr = new StreamReader(filename);
            var source = sr.ReadToEnd();
            sr.Close();

            id = GL.CreateShader(type);

            GL.ShaderSource(id, source);
            try { GL.CompileShader(id); }
            catch (Exception) { }
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
                throw new Exception("CompileShader failed");
#endif
        }

        public void Dispose()
        {
            GL.DeleteShader(id);
        }
    }
}