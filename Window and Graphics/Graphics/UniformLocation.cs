using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Graphics
{
    public class UniformLocation
    {
        public int id;

        public string name;

        public UniformLocation(Program program, string name)
        {
            id = GL.GetUniformLocation(program.id, name);
            this.name = name;
        }

        public void Set(float v0)
        {
            GL.Uniform1(id, v0);
        }
    }
}
