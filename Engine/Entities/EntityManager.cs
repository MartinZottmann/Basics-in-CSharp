using System;
using System.Collections.Generic;

namespace MartinZottmann.Engine.Entities
{
    public class EntityManager
    {
        protected Dictionary<string, Entity> entity_names = new Dictionary<string, Entity>();

        protected List<Entity> entities = new List<Entity>();

        protected Dictionary<Type, NodeList> node_lists = new Dictionary<Type, NodeList>();

        protected Dictionary<Type, ISystem> systems = new Dictionary<Type, ISystem>();

        public Entity[] Entities { get { return entities.ToArray(); } }

        public void Add(Entity entity)
        {
            entity_names.Add(entity.Name, entity);
            entities.Add(entity);
            entity.ComponentAdded += OnComponentAdded;
            entity.ComponentRemoved += OnComponentRemoved;
            foreach (var node_list in node_lists.Values)
                node_list.MaybeAdd(entity);
        }

        public void Add<T>(T system) where T : ISystem
        {
            var t = typeof(T);

            systems.Add(t, system);
            system.Bind(this);
        }

        public void Remove(Entity entity)
        {
            entity_names.Remove(entity.Name);
            entities.Remove(entity);
            entity.ComponentAdded -= OnComponentAdded;
            entity.ComponentRemoved -= OnComponentRemoved;
            foreach (var node_list in node_lists.Values)
                node_list.MaybeRemove(entity);
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
