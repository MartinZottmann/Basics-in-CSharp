namespace MartinZottmann.Engine.States
{
    public interface IStatable<TObject, TValue>
    {
        void Add(TValue value);

        void Remove(TValue value);
    }

    public interface IStatable<TObject, TKey, TValue>
    {
        void Add(TKey key, TValue value);

        void Remove(TKey key);
    }
}
