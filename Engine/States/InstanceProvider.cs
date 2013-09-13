using System;

namespace MartinZottmann.Engine.States
{
    public class InstanceProvider<T> : IProvider<T>
    {
        protected T instance;

        public InstanceProvider(T instance)
        {
            this.instance = instance;
        }

        public T Get()
        {
            return instance;
        }

        public bool Equals(IProvider<T> other)
        {
            return Object.Equals(Get(), other.Get());
        }
    }
}
