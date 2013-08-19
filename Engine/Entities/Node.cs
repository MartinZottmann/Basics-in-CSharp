using System;

namespace MartinZottmann.Engine.Entities
{
    public class Node
    {
        public Entity Entity;

        public override string ToString()
        {
            return String.Format("{0} for {1}", GetType().Name, Entity);
        }
    }
}
