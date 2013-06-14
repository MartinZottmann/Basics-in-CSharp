using System;
using System.Collections.Generic;

namespace MartinZottmann.Engine.Resources
{
    public abstract class ResourceManager<T> : IDisposable where T : IDisposable
    {
        public Resources Resources { get; protected set; }

        protected Dictionary<string, T> resources = new Dictionary<string, T>();

        public ResourceManager(Resources resources)
        {
            this.Resources = resources;
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
