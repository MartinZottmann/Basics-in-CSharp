using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MartinZottmann.Engine.Entities
{
    public class EntityManager : IDisposable
    {
        protected Dictionary<string, Entity> entity_names = new Dictionary<string, Entity>();

        protected List<Entity> entities = new List<Entity>();

        protected Dictionary<Type, NodeList> node_lists = new Dictionary<Type, NodeList>();

        protected Dictionary<Type, ISystem> systems = new Dictionary<Type, ISystem>();

        public Entity[] Entities { get { return entities.ToArray(); } }

        public void Dispose()
        {
            Debug.WriteLine("EntityManager.Disposing");
            foreach (var system in systems)
                if (system.Value is IDisposable)
                    ((IDisposable)system.Value).Dispose();
            Debug.WriteLine("EntityManager.Disposed");
        }

        public void Add(Entity entity)
        {
            if (entity_names.ContainsKey(entity.Name))
                return;

            entity_names.Add(entity.Name, entity);
            entities.Add(entity);
            entity.NameChanged += OnNameChanged;
            entity.ComponentAdded += OnComponentAdded;
            entity.ComponentRemoved += OnComponentRemoved;
            foreach (var node_list in node_lists.Values)
                node_list.MaybeAdd(entity);
        }

        public EntityManager Add<T>(T system) where T : ISystem
        {
            var t = typeof(T);

            systems.Add(t, system);
            system.Bind(this);

            return this;
        }

        public void Remove(Entity entity)
        {
            entity_names.Remove(entity.Name);
            entities.Remove(entity);
            entity.NameChanged -= OnNameChanged;
            entity.ComponentAdded -= OnComponentAdded;
            entity.ComponentRemoved -= OnComponentRemoved;
            foreach (var node_list in node_lists.Values)
                node_list.MaybeRemove(entity);
        }

        public void Remove(string name)
        {
            Remove(entity_names[name]);
        }

        public NodeList<T> Get<T>() where T : Node
        {
            var t = typeof(T);

            if (node_lists.ContainsKey(t))
                return (NodeList<T>)node_lists[t];

            var node_list = new NodeList<T>(t);
            node_lists[t] = node_list;
            foreach (var entity in entities)
                node_list.MaybeAdd(entity);

            return (NodeList<T>)node_lists[t];
        }

        public Entity Get(string name)
        {
            return entity_names[name];
        }

        public T GetSystem<T>() where T : ISystem
        {
            return (T)systems[typeof(T)];
        }

        protected void OnNameChanged(object sender, NameEventArgs e)
        {
            entity_names.Remove(e.OldName);
            entity_names.Add(e.NewName, e.Entity);
        }

        protected void OnComponentAdded(object sender, ComponentEventArgs e)
        {
            foreach (var node_list in node_lists.Values)
                node_list.ComponentAdded(e.Entity, e.ComponentType);
        }

        protected void OnComponentRemoved(object sender, ComponentEventArgs e)
        {
            foreach (var node_list in node_lists.Values)
                node_list.ComponentRemoved(e.Entity, e.ComponentType);
        }

        public void Update(double delta_time)
        {
            foreach (var system in systems.Values)
                system.Update(delta_time);
        }

        public void Render(double delta_time)
        {
            foreach (var system in systems.Values)
                system.Render(delta_time);
        }
    }
}
