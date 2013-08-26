using System;

namespace MartinZottmann.Engine.Entities
{
    public class NameEventArgs : EventArgs
    {
        public readonly Entity Entity;

        public readonly string OldName;

        public readonly string NewName;

        public NameEventArgs(Entity entity, string old_name, string new_name)
        {
            Entity = entity;
            OldName = old_name;
            NewName = new_name;
        }
    }
}
