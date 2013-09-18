using System;

namespace MartinZottmann.Engine.States
{
    public class TypeProvider<TValue> : IProvider<TValue>, IEquatable<TypeProvider<TValue>>
    {
        public Type Type { get; protected set; }

        public TValue Value { get { return (TValue)Activator.CreateInstance(Type); } }

        public TypeProvider(Type type)
        {
            Type = type;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (null == other)
                return false;

            if (!(other is TypeProvider<TValue>))
                return false;

            return Equals((TypeProvider<TValue>)other);
        }

        public bool Equals(IProvider<TValue> other)
        {
            if (null == other)
                return false;

            if (!(other is TypeProvider<TValue>))
                return false;

            return Equals((TypeProvider<TValue>)other);
        }

        public bool Equals(TypeProvider<TValue> other)
        {
            return Object.Equals(Type, other.Type);
        }

        public static bool operator ==(TypeProvider<TValue> a, TypeProvider<TValue> b)
        {
            if (null == a || null == b)
                return false;

            if (Object.ReferenceEquals(a, b))
                return true;

            return a.Equals(b);
        }

        public static bool operator !=(TypeProvider<TValue> a, TypeProvider<TValue> b)
        {
            return !(a == b);
        }
    }

    public class TypeProvider<TKey, TValue> : IProvider<TKey, TValue>, IEquatable<TypeProvider<TKey, TValue>>
    {
        public Type Type { get; protected set; }

        public TKey Key { get; protected set; }

        public TValue Value { get { return (TValue)Activator.CreateInstance(Type); } }

        public TypeProvider(TKey key, Type type)
        {
            Key = key;
            Type = type;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode() ^ Type.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (null == other)
                return false;

            if (!(other is TypeProvider<TKey, TValue>))
                return false;

            return Equals((TypeProvider<TKey, TValue>)other);
        }

        public bool Equals(IProvider<TKey, TValue> other)
        {
            if (null == other)
                return false;

            if (!(other is TypeProvider<TKey, TValue>))
                return false;

            return Equals((TypeProvider<TKey, TValue>)other);
        }

        public bool Equals(TypeProvider<TKey, TValue> other)
        {
            return Object.Equals(Key, other.Key) && Object.Equals(Type, other.Type);
        }

        public static bool operator ==(TypeProvider<TKey, TValue> a, TypeProvider<TKey, TValue> b)
        {
            if (null == a || null == b)
                return false;

            if (Object.ReferenceEquals(a, b))
                return true;

            return a.Equals(b);
        }

        public static bool operator !=(TypeProvider<TKey, TValue> a, TypeProvider<TKey, TValue> b)
        {
            return !(a == b);
        }
    }
}
