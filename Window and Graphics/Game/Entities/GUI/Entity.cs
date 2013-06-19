using MartinZottmann.Engine.Resources;

namespace MartinZottmann.Game.Entities.GUI
{
    public abstract class Entity : Entities.Entity
    {
        public Entity(ResourceManager resources) : base(resources) { }
    }
}
