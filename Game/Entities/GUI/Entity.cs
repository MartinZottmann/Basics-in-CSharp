using MartinZottmann.Engine.Resources;

namespace MartinZottmann.Game.Entities.GUI
{
    public abstract class Entity : Entities.Drawable
    {
        public Entity(ResourceManager resources) : base(resources) { }
    }
}
