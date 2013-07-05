using MartinZottmann.Engine.Resources;

namespace MartinZottmann.Game.Entities.Ships.Components
{
    public abstract class Component : Physical
    {
        public Component(ResourceManager resources) : base(resources) { }
    }
}
