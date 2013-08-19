using System;
using System.Collections.Generic;

namespace MartinZottmann.Engine.Entities
{
    [Serializable]
    public class Entity
    {
        protected static uint id = 0;

        public readonly string Name;

        [field: NonSerialized]
        public event EventHandler<ComponentEventArgs> ComponentAdded;

        [field: NonSerialized]
        public event EventHandler<ComponentEventArgs> ComponentRemoved;

        internal Dictionary<Type, IComponent> components = new Dictionary<Type, IComponent>();

        public Entity() : this("Entity", true) { }

        public Entity(string name, bool append_number = false)
        {
            id++;
            if (append_number)
                Name = name + " #" + id;
            else
                Name = name;
        }

        public Entity Add<T>(T component) where T : IComponent
        {
            components.Add(component.GetType(), component);
            if (ComponentAdded != null)
                ComponentAdded(this, new ComponentEventArgs(this, component.GetType()));

            return this;
        }

        public T Get<T>() where T : IComponent
        {
            return (T)components[typeof(T)];
        }

        public IComponent Get(Type component)
        {
            return components[component];
        }

        public void Remove<T>() where T : IComponent
        {
            components.Remove(typeof(T));
            if (ComponentRemoved != null)
                ComponentRemoved(this, new ComponentEventArgs(this, typeof(T)));
        }

        public void Remove(Type component)
        {
            components.Remove(component);
            if (ComponentRemoved != null)
                ComponentRemoved(this, new ComponentEventArgs(this, component));
        }

        public bool Has<T>() where T : IComponent
        {
            return components.ContainsKey(typeof(T));
        }

        public bool Has(Type component)
        {
            return components.ContainsKey(component);
        }

        public override string ToString()
        {
            return String.Format("{0} of type {1}", Name, base.ToString());
        }
    }
}
