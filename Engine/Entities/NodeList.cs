using System;
using System.Collections.Generic;

namespace MartinZottmann.Engine.Entities
{
    public class NodeList<T> : NodeList, IEnumerable<T> where T : Node
    {
        public readonly Type NodeType;

        protected HashSet<Type> component_types = new HashSet<Type>();

        protected List<T> nodes = new List<T>();

        public event EventHandler<NodeEventArgs<T>> NodeAdded;

        public event EventHandler<NodeEventArgs<T>> NodeRemoved;

        public T First { get { return nodes[0]; } }

        public T Last { get { return nodes[nodes.Count - 1]; } }

        public NodeList(Type node_type)
        {
            NodeType = node_type;
            foreach (var field in NodeType.GetFields())
                foreach (var @interface in field.FieldType.GetInterfaces())
                    if (@interface.FullName == typeof(IComponent).FullName)
                        component_types.Add(field.FieldType);
        }

        protected internal override void ComponentAdded(Entity entity, Type component_type)
        {
            if (!component_types.Contains(component_type))
                return;

            MaybeAdd(entity);
        }

        protected internal override void ComponentRemoved(Entity entity, Type component_type)
        {
            if (!component_types.Contains(component_type))
                return;

            var node = nodes.Find(s => s.Entity == entity);
            if (node == null)
                throw new Exception();

            nodes.Remove(node);
            if (NodeRemoved != null)
                NodeRemoved(this, new NodeEventArgs<T>(this, node));
        }

        protected internal override void MaybeAdd(Entity entity)
        {
            foreach (var i in component_types)
                if (!entity.Has(i))
                    return;

            var node = (T)Activator.CreateInstance(NodeType);
            node.Entity = entity;
            foreach (var i in node.GetType().GetFields())
                foreach (var j in component_types)
                    if (i.FieldType == j)
                    {
                        i.SetValue(node, entity.Get(j));
                        break;
                    }

            nodes.Add(node);
            if (NodeAdded != null)
                NodeAdded(this, new NodeEventArgs<T>(this, node));
        }

        protected internal override void MaybeRemove(Entity entity)
        {
            var node = nodes.Find(s => s.Entity == entity);
            if (node == null)
                return;

            nodes.Remove(node);
            if (NodeRemoved != null)
                NodeRemoved(this, new NodeEventArgs<T>(this, node));

            //foreach (var i in component_types)
            //    if (!entity.Has(i))
            //    {
            //        entities.Remove(entity);
            //        return;
            //    }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return nodes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return nodes.GetEnumerator();
        }
    }
}
