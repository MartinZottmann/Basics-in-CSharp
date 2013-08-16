using MartinZottmann.Game.Graphics;
using MartinZottmann.Game.IO;

namespace MartinZottmann.Game.Entities.Components
{
    public abstract class Abstract : IComponent
    {
        public GameObject GameObject { get; protected set; }

        public Abstract(GameObject game_object)
        {
            GameObject = game_object;
        }

        public virtual void Dispose() { }

        public virtual void Update(double delta_time, RenderContext render_context) { }

        public virtual void Render(double delta_time, RenderContext render_context) { }

        public virtual SaveValue SaveValue()
        {
            var status = new SaveValue(1);

            return status;
        }

        public virtual void Load(SaveValue status) { }
    }
}
