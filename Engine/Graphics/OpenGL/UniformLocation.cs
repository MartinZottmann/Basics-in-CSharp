﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Engine.Graphics.OpenGL
{
    public class UniformLocation
    {
        public readonly Program Program;

        public readonly uint Id;

        public readonly string Name;

        public UniformLocation(Program program, uint id, string name)
        {
            Program = program;
            Id = id;
            Name = name;
        }

        public UniformLocation(Program program, string name)
        {
            var id = GL.GetUniformLocation(program.Id, name);
            if (id == -1)
                throw new Exception(String.Format("GetUniformLocation failed for {0} in program {1}", name, program.Id));

            Program = program;
            Id = (uint)id;
            Name = name;
        }

        public void Set(int v0)
        {
            using (new Bind(Program))
                GL.Uniform1((int)Id, v0);
        }

        public void Set(float v0)
        {
            using (new Bind(Program))
                GL.Uniform1((int)Id, v0);
        }

        public void Set(Vector3 v0)
        {
            using (new Bind(Program))
                GL.Uniform3((int)Id, v0);
        }

        public void Set(Vector3d v0)
        {
            using (new Bind(Program))
                GL.Uniform3((int)Id, v0.X, v0.Y, v0.Z);
        }

        public void Set(Vector4 v0)
        {
            using (new Bind(Program))
                GL.Uniform4((int)Id, v0);
        }

        public void Set(OpenTK.Graphics.Color4 c0)
        {
            using (new Bind(Program))
                GL.Uniform4((int)Id, c0);
        }

        public void Set(Matrix4 m0)
        {
            using (new Bind(Program))
                GL.UniformMatrix4((int)Id, false, ref m0);
        }

        public void Set(float[] f0)
        {
            if (f0.Length == 1)
                GL.Uniform1((int)Id, f0[0]);
            else if (f0.Length == 2)
                GL.Uniform2((int)Id, f0[0], f0[1]);
            else if (f0.Length == 3)
                GL.Uniform3((int)Id, f0[0], f0[1], f0[2]);
            else if (f0.Length == 4)
                GL.Uniform4((int)Id, f0[0], f0[1], f0[2], f0[3]);
            else if (f0.Length == 9)
                GL.UniformMatrix3((int)Id, 9, false, f0);
        }

        public void Set(Matrix4d m0)
        {
            using (new Bind(Program))
                GL.UniformMatrix4((int)Id, false, ref m0);
        }
    }
}