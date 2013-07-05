using MartinZottmann.Engine.Graphics;
using OpenTK;

namespace MartinZottmann.Game.Graphics
{
    public class RenderContext : RenderContext<RenderContext>
    {
        public new RenderContext Parent;

        public bool Debug;

        public override RenderContext Push()
        {
            var render_context = base.Push();
            render_context.Debug = Debug;
            return render_context;
            //return new RenderContext()
            //{
            //    Parent = this,
            //    Debug = Debug,
            //    Window = Window,
            //    Camera = Camera,
            //    Projection = Matrix4d.Identity,
            //    View = Matrix4d.Identity,
            //    Model = Matrix4d.Identity
            //};
        }

        //public new RenderContext Pop()
        //{
        //    return Parent;
        //}
    }
}
