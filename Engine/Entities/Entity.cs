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

        [XmlArray("Components")]
        [XmlArrayItem("Component")]
        public object[] Components
        {
            get
            {
                IComponent[] r = new IComponent[components.Count];
                components.Values.CopyTo(r, 0);
                return r;
            }
            set
            {
                foreach (var component in value)
                    Add((IComponent)component);
            }
        }

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

        public void Remove<T>() where T : IComponent
        {
            components.Remove(typeof(T));
            if (null != ComponentRemoved)
                ComponentRemoved(this, new ComponentEventArgs(this, typeof(T)));
        }

        public void Remove(Type component)
        {
            components.Remove(component);
            if (null != ComponentRemoved)
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
