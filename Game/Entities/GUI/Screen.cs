using MartinZottmann.Engine.Resources;

namespace MartinZottmann.Game.Entities.GUI
{
    public class Screen : MartinZottmann.Game.Entities.Entity
    {
        public Screen(ResourceManager resources) : base(resources) { }

        public override void Update(double delta_time, Graphics.RenderContext render_context)
        {
            children.ForEach(s => s.Update(delta_time, render_context));
        }

        public override void Render(double delta_time, Graphics.RenderContext render_context)
        {
            children.ForEach(s => s.Render(delta_time, render_context));
        }
    }
}
