namespace MartinZottmann.Game.Entities.Components
{
    public abstract class Abstract
    {
        public Entity Entity { get; protected set; }

        public Abstract(Entity entity)
        {
            Entity = entity;
        }
    }
}
