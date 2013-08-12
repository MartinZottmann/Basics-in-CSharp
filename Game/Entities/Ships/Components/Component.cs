using MartinZottmann.Engine.Resources;

namespace MartinZottmann.Game.Entities.Ships.Components
{
    public abstract class Component : GameObject
    {
        public Component(ResourceManager resources) : base(resources) { }
    }
}
