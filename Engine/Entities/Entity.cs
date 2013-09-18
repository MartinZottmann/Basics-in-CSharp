using MartinZottmann.Engine.States;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MartinZottmann.Engine.Entities
{
    [Serializable]
    public class Entity
    {
        protected static uint id = 0;

        protected internal string name;

        [XmlAttribute]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                var old_name = name;
                name = value;
                if (null != NameChanged)
                    NameChanged(this, new NameEventArgs(this, old_name, name));
            }
        }

        [field: NonSerialized]
        public event EventHandler<NameEventArgs> NameChanged;

        [field: NonSerialized]
        public event EventHandler<ComponentEventArgs> ComponentAdded;

        [field: NonSerialized]
        public event EventHandler<ComponentEventArgs> ComponentRemoved;

        protected internal Dictionary<Type, IComponent> components = new Dictionary<Type, IComponent>();

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
            if (null != ComponentAdded)
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

        public Entity Remove<T>() where T : IComponent
        {
            components.Remove(typeof(T));
            if (null != ComponentRemoved)
                ComponentRemoved(this, new ComponentEventArgs(this, typeof(T)));

            return this;
        }

        public Entity Remove(IComponent component)
        {
            var type = component.GetType();
            components.Remove(type);
            if (null != ComponentRemoved)
                ComponentRemoved(this, new ComponentEventArgs(this, type));

            return this;
        }

        public Entity Remove(Type component)
        {
            components.Remove(component);
            if (null != ComponentRemoved)
                ComponentRemoved(this, new ComponentEventArgs(this, component));

            return this;
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

        public StateMachine<Entity, IComponent> GetComponentStateMachine()
        {
            return new StateMachine<Entity, IComponent>(
                this,
                (IComponent s) => Add(s),
                (IComponent s) => Remove(s.GetType())
            );
        }
    }
}
