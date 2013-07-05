using MartinZottmann.Engine.Graphics;
using OpenTK;
using System.Collections.Generic;

namespace MartinZottmann.Game.Graphics
{
    public class RenderContext : RenderContext<RenderContext>
    {
        public new RenderContext Parent;

        public bool Debug;

        public float alpha_cutoff;

        public override RenderContext Push()
        {
            var render_context = base.Push();
            render_context.Debug = Debug;
            return render_context;
        }
    }
}
