using MartinZottmann.Engine.States;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MartinZottmann.Engine.Entities
{
    public class EntityManager : IStatable<EntityManager, Entity>, IStatable<EntityManager, ISystem>
    {
        protected Dictionary<string, Entity> entity_names = new Dictionary<string, Entity>();

        protected List<Entity> entities = new List<Entity>();

        protected Dictionary<Type, INodeList> node_lists = new Dictionary<Type, INodeList>();

        protected Dictionary<Type, ISystem> systems = new Dictionary<Type, ISystem>();

        public Entity[] Entities { get { return entities.ToArray(); } }

        public void Clear()
        {
            ClearSystems();
            ClearEntites();
        }

        public void AddEntity(Entity entity)
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

        public Entity GetEntity(string name)
        {
            return entity_names[name];
        }

        public void RemoveEntity(Entity entity)
        {
            entity.NameChanged -= OnNameChanged;
            entity.ComponentAdded -= OnComponentAdded;
            entity.ComponentRemoved -= OnComponentRemoved;
            foreach (var node_list in node_lists.Values)
                node_list.MaybeRemove(entity);
            entities.Remove(entity);
            entity_names.Remove(entity.Name);
        }

        public void RemoveEntity(string name)
        {
            RemoveEntity(entity_names[name]);
        }

        public void ClearEntites()
        {
            foreach (var entity in entities)
            {
                entity.NameChanged -= OnNameChanged;
                entity.ComponentAdded -= OnComponentAdded;
                entity.ComponentRemoved -= OnComponentRemoved;
                foreach (var node_list in node_lists.Values)
                    node_list.MaybeRemove(entity);
            }
            entities.Clear();
            entity_names.Clear();
        }

        public NodeList<T> GetNodeList<T>() where T : Node
        {
            var type = typeof(T);

            if (node_lists.ContainsKey(type))
                return (NodeList<T>)node_lists[type];

            var node_list = new NodeList<T>();
            node_lists[type] = node_list;
            foreach (var entity in entities)
                node_list.MaybeAdd(entity);

            return (NodeList<T>)node_lists[type];
        }

        public void AddSystem<T>(T system) where T : ISystem
        {
            var type = typeof(T);

            systems.Add(type, system);
            system.Start(this);
        }

        public T GetSystem<T>() where T : ISystem
        {
            var type = typeof(T);

            return (T)systems[type];
        }

        public void RemoveSystem(ISystem system)
        {
            var type = system.GetType();

            if (system != systems[type])
                throw new Exception();
            system.Stop();
            systems.Remove(type);
        }

        public void RemoveSystem(Type type)
        {
            var system = systems[type];
            system.Stop();
            systems.Remove(type);
        }

        public void RemoveSystem<T>() where T : ISystem
        {
            var type = typeof(T);

            var system = systems[type];
            system.Stop();
            systems.Remove(type);
        }

        public void ClearSystems()
        {
            Debug.WriteLine("Systems Clearing", GetType().FullName);
            Debug.WriteLine("Systems Stopping", GetType().FullName);
            foreach (var system in systems.Values)
                system.Stop();
            Debug.WriteLine("Systems Stopped", GetType().FullName);
            systems.Clear();
            Debug.WriteLine("Systems Cleared", GetType().FullName);
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

        #region IStatable<EntityManager,Entity> Members

        void IStatable<EntityManager, Entity>.Add(Entity value)
        {
            AddEntity(value);
        }

        void IStatable<EntityManager, Entity>.Remove(Entity value)
        {
            RemoveEntity(value);
        }

        #endregion

        #region IStatable<EntityManager,ISystem> Members

        void IStatable<EntityManager, ISystem>.Add(ISystem value)
        {
            AddSystem(value);
        }

        void IStatable<EntityManager, ISystem>.Remove(ISystem value)
        {
            RemoveSystem(value);
        }

        #endregion
    }
}
