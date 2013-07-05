using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.IO;

namespace MartinZottmann.Game.Entities.GUI
{
    public abstract class Entity : Entities.Drawable
    {
        public Entity(ResourceManager resources) : base(resources) { }

        public override SaveValue SaveValue()
        {
            return null;
        }
    }
}
