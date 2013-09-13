using System;
using System.Collections.Generic;

namespace MartinZottmann.Engine.States
{
    [Serializable]
    public class State<T>
    {
        protected Dictionary<Type, IProvider<T>> providers = new Dictionary<Type, IProvider<T>>();

        public Dictionary<Type, IProvider<T>> Providers { get { return new Dictionary<Type, IProvider<T>>(providers); } }

        public State<T> Add(T instance)
        {
            providers.Add(instance.GetType(), new InstanceProvider<T>(instance));
            return this;
        }

        public State<T> Add(Type type)
        {
            providers.Add(type, new TypeProvider<T>(type));
            return this;
        }

        public State<T> Add<U>() where U : class
        {
            var type = typeof(U);
            providers.Add(type, new TypeProvider<T>(type));
            return this;
        }

        public State<T> Remove(T instance)
        {
            providers.Remove(instance.GetType());
            return this;
        }

        public State<T> Remove(Type type)
        {
            providers.Remove(type);
            return this;
        }
    }
}
