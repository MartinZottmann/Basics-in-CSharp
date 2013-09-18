using System;
using System.Collections.Generic;

namespace MartinZottmann.Engine.States
{
    [Serializable]
    public class State<TValue>
    {
        public HashSet<IProvider<TValue>> Providers = new HashSet<IProvider<TValue>>();

        public HashSet<IProvider<TValue>> ProvidersCloneFlat()
        {
            return new HashSet<IProvider<TValue>>(Providers);
        }

        public State<TValue> Add(TValue value)
        {
            Providers.Add(new InstanceProvider<TValue>(value));
            return this;
        }

        public State<TValue> Add(Type type)
        {
            Providers.Add(new TypeProvider<TValue>(type));
            return this;
        }

        public State<TValue> Add<T>() where T : TValue
        {
            Providers.Add(new TypeProvider<TValue>(typeof(T)));
            return this;
        }
    }

    [Serializable]
    public class State<TKey, TValue>
    {
        public HashSet<IProvider<TKey, TValue>> Providers = new HashSet<IProvider<TKey, TValue>>();

        public HashSet<IProvider<TKey, TValue>> ProvidersCloneFlat()
        {
            return new HashSet<IProvider<TKey, TValue>>(Providers);
        }

        public State<TKey, TValue> Add(TKey key, TValue value)
        {
            Providers.Add(new InstanceProvider<TKey, TValue>(key, value));
            return this;
        }

        public State<TKey, TValue> Add(TKey key, Type type)
        {
            Providers.Add(new TypeProvider<TKey, TValue>(key, type));
            return this;
        }
    }
}
