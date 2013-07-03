namespace MartinZottmann.Engine.Physics
{
    public interface ICollidable
    {
        Collision Collides(object @object);
    }

    public interface ICollidable<T>
    {
        Collision Collides(T @object);

        //Collision Collides(ref T @object);
    }
}
