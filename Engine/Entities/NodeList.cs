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
        public class FieldMeta
        {
            public Type FieldType;

            public Action<object, object> SetValue;

            public bool Required;
        }

        protected Delegate node_contructor;

        protected IEnumerable<FieldMeta> fields;

        protected List<T> nodes = new List<T>();

        public T[] Nodes { get { return nodes.ToArray(); } }

        public T First { get { return nodes[0]; } }

        public T Last { get { return nodes[nodes.Count - 1]; } }

        public event EventHandler<NodeEventArgs<T>> NodeAdded;

        public event EventHandler<NodeEventArgs<T>> NodeRemoved;

        public NodeList()
        {
            node_contructor = Expression.Lambda(Expression.New(typeof(T))).Compile();

            fields = typeof(T)
                .GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(s => typeof(IComponent).IsAssignableFrom(s.FieldType))
                .Select(
                    s => new FieldMeta()
                    {
                        FieldType = s.FieldType,
                        Required = null == s.GetCustomAttribute<OptionalComponentAttribute>(),
                        SetValue = s.SetValue
                    }
                );
        }

        public void ComponentAdded(Entity entity, Type component_type)
        {
            if (!fields.Any(s => s.FieldType == component_type))
                return;

            MaybeAdd(entity);
        }

        public void ComponentRemoved(Entity entity, Type component_type)
        {
            if (!fields.Any(s => s.FieldType == component_type))
                return;

            MaybeRemove(entity);
        }

        public void MaybeAdd(Entity entity)
        {
            if (fields.Any(s => s.Required && !entity.Has(s.FieldType)))
                return;

            var node = (T)node_contructor.DynamicInvoke();
            node.Entity = entity;
            foreach (var field in fields.Where(s => entity.Has(s.FieldType)))
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

            foreach (var field in fields.Where(s => !entity.Has(s.FieldType)))
                if (field.Required)
                {
                    nodes.Remove(node);
                    if (null != NodeRemoved)
                        NodeRemoved(this, new NodeEventArgs<T>(this, node));
                    break;
                }
                else
                    field.SetValue(node, null);
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
