using MartinZottmann.Game.Entities;

namespace MartinZottmann.Game.AI
{
    public abstract class Base
    {
        public Entity Entity;

        public Base(Entity entity)
        {
            Entity = entity;
        }

        public virtual void Update(double delta_time) { }
    }
}
