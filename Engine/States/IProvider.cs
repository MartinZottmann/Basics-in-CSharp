using System;

namespace MartinZottmann.Engine.States
{
    public interface IProvider<T> : IEquatable<IProvider<T>>
    {
        T Get();
    }
}
