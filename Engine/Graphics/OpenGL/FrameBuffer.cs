using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Engine.Graphics.OpenGL
{
    public class FrameBuffer : IBindable, IDisposable
    {
        public readonly uint Id;

        public readonly FramebufferTarget Target;

        public FrameBuffer(FramebufferTarget target)
        {
            Id = (uint)GL.GenFramebuffer();
            Target = target;
        }

        public void Texture(FramebufferAttachment attachment, Texture texture, int level)
        {
            using (new Bind(this))
                GL.FramebufferTexture(Target, attachment, texture.Id, level);
        }

        public void DrawBuffer(DrawBufferMode mode)
        {
            using (new Bind(this))
                GL.DrawBuffer(mode);
        }

        public void ReadBuffer(ReadBufferMode mode)
        {
            using (new Bind(this))
                GL.ReadBuffer(mode);
        }

        public void CheckStatus()
        {
            using (new Bind(this))
            {
                var status = GL.CheckFramebufferStatus(Target);
                if (status != FramebufferErrorCode.FramebufferComplete)
                    throw new Exception(status.ToString());
            }
        }

        public void Bind()
        {
            GL.BindFramebuffer(Target, Id);
        }

        public void UnBind()
        {
            GL.BindFramebuffer(Target, 0);
        }

        public void Dispose()
        {
            GL.DeleteFramebuffer(Id);
        }
    }
}
