using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MartinZottmann.Game.IO
{
    [Serializable]
    public class SaveObject : Dictionary<string, object>, ISerializable
    {
        public SaveObject() : base() { }

        public SaveObject(SerializationInfo info, StreamingContext context) : base(info, context) { }

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
    }
}
