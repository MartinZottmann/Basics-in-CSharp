using System;

namespace MartinZottmann.Game.IO
{
    [Serializable]
    public class SaveKey
    {
        public readonly int HashCode;

        public readonly Type Type;

        public readonly string String;

        public SaveKey(object @object)
        {
            HashCode = @object.GetHashCode();
            Type = @object.GetType();
            String = @object.ToString();
        }
    }
}
