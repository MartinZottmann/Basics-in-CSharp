using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace MartinZottmann.Graphics
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
            Debug.Assert(id != -1, "GetUniformLocation failed");
        }

        public void Set(float v0)
        {
            using (new Bind(program))
                GL.Uniform1(id, v0);
        }
    }
}
