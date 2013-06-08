using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace MartinZottmann.Graphics
{
    public class UniformBlockIndex
    {
        public Program program;

        public int id;

        public string name;

        public UniformBlockIndex(Program program, string name)
        {
            this.program = program;
            this.name = name;

            id = GL.GetUniformBlockIndex(program.id, name);
            // @todo This seems bugged (Driver problem?)
            Debug.Assert(id != -1, "GetUniformBlockIndex failed");
            int size;
            GL.GetActiveUniformBlock(program.id, id, ActiveUniformBlockParameter.UniformBlockDataSize, out size);
#if DEBUG
            MartinZottmann.Program.OpenGLDebug();
#endif
        }
    }
}
