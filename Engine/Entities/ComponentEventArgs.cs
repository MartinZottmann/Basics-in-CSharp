using System;

namespace MartinZottmann.Engine.Entities
{
    public class ComponentEventArgs : EventArgs
    {
        public readonly Entity Entity;

        public readonly Type ComponentType;

        public ComponentEventArgs(Entity entity, Type component_type)
        {
            Entity = entity;
            ComponentType = component_type;
        }
    }
}
