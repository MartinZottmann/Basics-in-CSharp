using System;
using System.Collections.Generic;

namespace MartinZottmann.Engine.Resources
{
    public abstract class Resource<T> : IDisposable where T : IDisposable
    {
        public ResourceManager ResourceManager { get; protected set; }

        protected Dictionary<string, T> resources = new Dictionary<string, T>();

        public int Count { get { return resources.Count; } }

        public Resource(ResourceManager resource_manager)
        {
            ResourceManager = resource_manager;
        }

        public T this[string key]
        {
            get
            {
                return resources[key];
            }
            set
            {
                resources[key] = value;
            }
        }

        public void Clear()
        {
            foreach (var x in resources.Values)
                x.Dispose();

            resources.Clear();
        }

        public void Dispose()
        {
            Clear();
        }

        //public abstract void LoadFromFile(string filename);
    }
}
