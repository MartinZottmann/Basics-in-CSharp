using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Engine.Graphics.OpenGL
{
    public class UniformBlockIndex
    {
        public readonly Program Program;

        public readonly uint Id;

        public readonly string Name;

        public bool Filled { get; protected set; }

        public UniformBlockIndex(Program program, uint id, string name)
        {
            Program = program;
            Id = id;
            Name = name;
            Filled = false;
        }

        public UniformBlockIndex(Program program, string name)
        {
            var id = GL.GetUniformBlockIndex(program.Id, name);
            if (id == -1)
                throw new Exception(String.Format("GetUniformBlockIndex failed for {0} in program {1}", name, program.Id));

            Program = program;
            Id = (uint)id;
            Name = name;
            Filled = false;

            int size;
            GL.GetActiveUniformBlock(program.Id, this.Id, ActiveUniformBlockParameter.UniformBlockDataSize, out size);
        }
    }
}
