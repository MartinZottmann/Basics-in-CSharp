using System;

namespace MartinZottmann.Engine.States
{
    public interface IStatable<T>
    {
        void Add(T instance);

        void Remove(T instance);

        void Remove(Type type);
    }
}
