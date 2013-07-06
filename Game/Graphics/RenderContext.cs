using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.OpenGL;
using OpenTK;

namespace MartinZottmann.Game.Graphics
{
    public class RenderContext : RenderContext<RenderContext>
    {
        public new RenderContext Parent;

        public bool Debug;

        public float alpha_cutoff;

        public MartinZottmann.Engine.Graphics.OpenGL.Program Program;

        public Texture DepthTexture;

        public override RenderContext Push()
        {
            var render_context = base.Push();
            render_context.Debug = Debug;
            render_context.alpha_cutoff = alpha_cutoff;
            render_context.Program = Program;
            return render_context;
        }
    }
}
