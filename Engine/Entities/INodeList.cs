using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartinZottmann.Engine.Entities
{
    abstract public class NodeList
    {
        abstract protected internal void ComponentAdded(Entity entity, Type component_type);

        abstract protected internal void ComponentRemoved(Entity entity, Type component_type);

        abstract protected internal void MaybeAdd(Entity entity);

        abstract protected internal void MaybeRemove(Entity entity);
    }
}
