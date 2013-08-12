using RenderContext = MartinZottmann.Game.Graphics.RenderContext;

namespace MartinZottmann.Game.Entities.Components
{
    public abstract class Abstract
    {
        public GameObject GameObject { get; protected set; }

        public Abstract(GameObject game_object)
        {
            GameObject = game_object;
        }

        public virtual void Update(double delta_time, RenderContext render_context) { }

        public virtual void Render(double delta_time, RenderContext render_context) { }
    }
}
