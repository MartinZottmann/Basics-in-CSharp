using System;
using System.Collections;

namespace MartinZottmann.Engine.Entities
{
    public interface INodeList : IEnumerable
    {
        void ComponentAdded(Entity entity, Type component_type);

        void ComponentRemoved(Entity entity, Type component_type);

        void MaybeAdd(Entity entity);

        void MaybeRemove(Entity entity);
    }
}
