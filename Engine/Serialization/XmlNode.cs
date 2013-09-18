using System;
using System.Collections.Generic;

namespace MartinZottmann.Engine.Serialization
{
    public class XmlNode
    {
        public bool IsEmptyElement;

        public Type Type;

        public Dictionary<string, string> Attributes;

        public Dictionary<string, object> Values;
    }
}
