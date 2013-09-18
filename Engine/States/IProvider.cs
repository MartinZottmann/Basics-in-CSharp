using System;

namespace MartinZottmann.Engine.States
{
    public interface IProvider<TValue> : IEquatable<IProvider<TValue>>
    {
        TValue Value { get; }
    }

    public interface IProvider<TKey, TValue> : IEquatable<IProvider<TKey, TValue>>
    {
        TKey Key { get; }

        TValue Value { get; }
    }
}
