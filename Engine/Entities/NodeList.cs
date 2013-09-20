using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MartinZottmann.Engine.Entities
{
    public class NodeList<T> : INodeList, IEnumerable<T> where T : Node
    {
        protected Delegate node_contructor;

        protected IEnumerable<FieldInfo> fields;

        protected Dictionary<Type, bool> component_types = new Dictionary<Type, bool>();

        protected List<T> nodes = new List<T>();

        public event EventHandler<NodeEventArgs<T>> NodeAdded;

        public event EventHandler<NodeEventArgs<T>> NodeRemoved;

        public T First { get { return nodes[0]; } }

        public T Last { get { return nodes[nodes.Count - 1]; } }

        public NodeList()
        {
            node_contructor = Expression.Lambda(Expression.New(typeof(T))).Compile();

            fields = typeof(T)
                .GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(s => typeof(IComponent).IsAssignableFrom(s.FieldType));

            foreach (var field in fields)
                component_types.Add(field.FieldType, null == field.GetCustomAttribute<OptionalComponentAttribute>());
        }

        public void ComponentAdded(Entity entity, Type component_type)
        {
            if (!component_types.ContainsKey(component_type))
                return;

            MaybeAdd(entity);
        }

        public void ComponentRemoved(Entity entity, Type component_type)
        {
            if (!component_types.ContainsKey(component_type))
                return;

            var node = nodes.Find(s => s.Entity == entity);
            if (null == node)
                throw new Exception();

            nodes.Remove(node);
            if (null != NodeRemoved)
                NodeRemoved(this, new NodeEventArgs<T>(this, node));
        }

        public void MaybeAdd(Entity entity)
        {
            foreach (var component_type in component_types)
                if (component_type.Value && !entity.Has(component_type.Key))
                    return;

            var node = (T)node_contructor.DynamicInvoke();
            node.Entity = entity;
            foreach (var field in fields)
                if (entity.Has(field.FieldType))
                    field.SetValue(node, entity.Get(field.FieldType));

            nodes.Add(node);
            if (null != NodeAdded)
                NodeAdded(this, new NodeEventArgs<T>(this, node));
        }

        public void MaybeRemove(Entity entity)
        {
            var node = nodes.Find(s => s.Entity == entity);
            if (null == node)
                return;

            nodes.Remove(node);
            if (null != NodeRemoved)
                NodeRemoved(this, new NodeEventArgs<T>(this, node));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
