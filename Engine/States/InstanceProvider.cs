using System;

namespace MartinZottmann.Engine.States
{
    [Serializable]
    public class InstanceProvider<TValue> : IProvider<TValue>, IEquatable<InstanceProvider<TValue>>
    {
        protected TValue value;

        public TValue Value { get { return value; } protected set { this.value = value; } }

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

    [Serializable]
    public class InstanceProvider<TKey, TValue> : IProvider<TKey, TValue>, IEquatable<InstanceProvider<TKey, TValue>>
    {
        protected TKey key;

        public TKey Key { get { return key; } protected set { key = value; } }

        protected TValue value;

        public TValue Value { get { return value; } protected set { this.value = value; } }

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
