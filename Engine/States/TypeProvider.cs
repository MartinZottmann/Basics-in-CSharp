using System;

namespace MartinZottmann.Engine.States
{
    public class TypeProvider<T> : IProvider<T>
    {
        protected Type type;

        public TypeProvider(Type type)
        {
            this.type = type;
        }

        public T Get()
        {
            return (T)Activator.CreateInstance(type);
        }

        public bool Equals(IProvider<T> other)
        {
            return Object.Equals(Get(), other.Get());
        }
    }
}
