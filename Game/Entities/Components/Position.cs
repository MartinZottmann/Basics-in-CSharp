using OpenTK;
using System;

namespace MartinZottmann.Game.Entities.Components
{
    [Serializable]
    public class Position : Abstract
    {
        protected Vector3d position;

        public Vector3d Get()
        {
            return position;
        }

        public void Set(Vector3d position)
        {
            this.position = position;
        }
    }
}
