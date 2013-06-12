using System;
using System.Collections.Generic;

namespace MartinZottmann.Engine
{
    public abstract class ResourceManager<T> where T : IDisposable
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
            // @todo IDisposeable?
            resources.Clear();
        }

        //public abstract void LoadFromFile(string filename);
    }
}
