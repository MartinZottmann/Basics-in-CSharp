using System;

namespace MartinZottmann.Engine.States
{
    public class InstanceProvider<TValue> : IProvider<TValue>, IEquatable<InstanceProvider<TValue>>
    {
        public TValue Value { get; protected set; }

        public InstanceProvider(TValue value)
        {
            Value = value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (null == other)
                return false;

            if (!(other is InstanceProvider<TValue>))
                return false;

            return Equals((InstanceProvider<TValue>)other);
        }

        public bool Equals(IProvider<TValue> other)
        {
            if (null == other)
                return false;

            if (!(other is InstanceProvider<TValue>))
                return false;

            return Equals((InstanceProvider<TValue>)other);
        }

        public bool Equals(InstanceProvider<TValue> other)
        {
            return Object.Equals(Value, other.Value);
        }

        public static bool operator ==(InstanceProvider<TValue> a, InstanceProvider<TValue> b)
        {
            if (null == a || null == b)
                return false;

            if (Object.ReferenceEquals(a, b))
                return true;

            return a.Equals(b);
        }

        public static bool operator !=(InstanceProvider<TValue> a, InstanceProvider<TValue> b)
        {
            return !(a == b);
        }
    }

    public class InstanceProvider<TKey, TValue> : IProvider<TKey, TValue>, IEquatable<InstanceProvider<TKey, TValue>>
    {
        public TKey Key { get; protected set; }

        public TValue Value { get; protected set; }

        public InstanceProvider(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode() ^ Value.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (null == other)
                return false;

            if (!(other is InstanceProvider<TKey, TValue>))
                return false;

            return Equals((InstanceProvider<TKey, TValue>)other);
        }

        public bool Equals(IProvider<TKey, TValue> other)
        {
            if (null == other)
                return false;

            if (!(other is InstanceProvider<TKey, TValue>))
                return false;

            return Equals((InstanceProvider<TKey, TValue>)other);
        }

        public bool Equals(InstanceProvider<TKey, TValue> other)
        {
            return Object.Equals(Key, other.Key) && Object.Equals(Value, other.Value);
        }

        public static bool operator ==(InstanceProvider<TKey, TValue> a, InstanceProvider<TKey, TValue> b)
        {
            if (null == a || null == b)
                return false;

            if (Object.ReferenceEquals(a, b))
                return true;

            return a.Equals(b);
        }

        public static bool operator !=(InstanceProvider<TKey, TValue> a, InstanceProvider<TKey, TValue> b)
        {
            return !(a == b);
        }
    }
}
