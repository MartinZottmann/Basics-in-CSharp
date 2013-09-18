using System.Collections.Generic;

namespace MartinZottmann.Engine.Serialization
{
    public class ObjectReference
    {
        protected object _lock = new object();

        protected Dictionary<object, int> references = new Dictionary<object, int>();

        public int Count
        {
            get
            {
                lock (_lock)
                    return references.Count;
            }
        }

        public int Add(object @object)
        {
            lock (_lock)
            {
                var id = references.Count + 1;
                references.Add(@object, id);
                return id;
            }
        }

        public int GetID(object @object)
        {
            lock (_lock)
                return references[@object];
        }

        public string GetIDValue(object @object)
        {
            lock (_lock)
                return GetID(@object).ToString();
        }

        public bool Contains(object @object)
        {
            lock (_lock)
                return references.ContainsKey(@object);
        }

        public void Clear()
        {
            lock (_lock)
                references.Clear();
        }
    }
}
