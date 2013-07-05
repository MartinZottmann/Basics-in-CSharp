using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MartinZottmann.Game.IO
{
    [Serializable]
    public class SaveValue : Dictionary<string, object>, ISerializable
    {
        public readonly int Version;

        public SaveValue(int version)
            : base()
        {
            Version = version;
        }

        public SaveValue(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public bool TryGetValue<T>(string key, ref T value)
        {
            object temp;
            if (TryGetValue(key, out temp))
            {
                value = (T)temp;
                return true;
            }

            return false;
        }

        public bool TryPop<T>(string key, ref T value)
        {
            object temp;
            if (TryGetValue(key, out temp))
            {
                Remove(key);
                value = (T)temp;
                return true;
            }

            return false;
        }
    }
}
