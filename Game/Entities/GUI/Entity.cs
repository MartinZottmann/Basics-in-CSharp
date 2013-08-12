using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.IO;

namespace MartinZottmann.Game.Entities.GUI
{
    public abstract class Entity : GameObject
    {
        public Entity(ResourceManager resources)
            : base(resources)
        {
            AddComponent(new Graphic(this));
        }

        public override SaveValue SaveValue()
        {
            return null;
        }
    }
}
