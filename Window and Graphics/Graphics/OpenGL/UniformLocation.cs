using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace MartinZottmann.Graphics.OpenGL
{
    public class UniformLocation
    {
        public Program program;

        public int id;

        public string name;

        public UniformLocation(Program program, string name)
        {
            this.program = program;
            this.name = name;

            id = GL.GetUniformLocation(program.id, name);
            if (id == -1)
            {
                OpenGL.Info.Uniform(id);
                Debug.Assert(id != -1, "GetUniformLocation failed");
            }
        }

        public void Set(int v0)
        {
            using (new Bind(program))
                GL.Uniform1(id, v0);
        }

        public void Set(float v0)
        {
            using (new Bind(program))
                GL.Uniform1(id, v0);
        }

        public void Set(Vector3 v0)
        {
            using (new Bind(program))
                GL.Uniform3(id, v0);
        }

        public void Set(Vector3d v0)
        {
            using (new Bind(program))
                GL.Uniform3(id, v0.X, v0.Y, v0.Z);
        }

        public void Set(Vector4 v0)
        {
            using (new Bind(program))
                GL.Uniform4(id, v0);
        }

        public void Set(OpenTK.Graphics.Color4 c0)
        {
            using (new Bind(program))
                GL.Uniform4(id, c0);
        }

        public void Set(Matrix4 m0)
        {
            using (new Bind(program))
                GL.UniformMatrix4(id, false, ref m0);
        }

        public void Set(float[] f0)
        {
            if (f0.Length == 1)
                GL.Uniform1(id, f0[0]);
            else if (f0.Length == 2)
                GL.Uniform2(id, f0[0], f0[1]);
            else if (f0.Length == 3)
                GL.Uniform3(id, f0[0], f0[1], f0[2]);
            else if (f0.Length == 4)
                GL.Uniform4(id, f0[0], f0[1], f0[2], f0[3]);
            else if (f0.Length == 9)
                GL.UniformMatrix3(id, 9, false, f0);
        }

        public void Set(Matrix4d m0)
        {
            var m1 = new Matrix4(
                (float)m0.M11, (float)m0.M12, (float)m0.M13, (float)m0.M14,
                (float)m0.M21, (float)m0.M22, (float)m0.M23, (float)m0.M24,
                (float)m0.M31, (float)m0.M32, (float)m0.M33, (float)m0.M34,
                (float)m0.M41, (float)m0.M42, (float)m0.M43, (float)m0.M44
            );
            using (new Bind(program))
                GL.UniformMatrix4(id, false, ref m1);
        }
    }
}
