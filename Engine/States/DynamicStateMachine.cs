using System;
using System.Collections.Generic;

namespace MartinZottmann.Engine.States
{
    public class DynamicStateMachine<TObject, TValue> : AbstractStateMachine<TObject, TValue>
    {
        public Action<TValue> Add;

        public Action<TValue> Remove;

        public DynamicStateMachine(TObject @object, Action<TValue> add, Action<TValue> remove)
            : base(@object)
        {
            this.Add = add;
            this.Remove = remove;
        }

        protected override void AddProviderToObject(IProvider<TValue> provider)
        {
            Add(provider.Value);
        }

        protected override void RemoveProviderFromObject(IProvider<TValue> provider)
        {
            Remove(provider.Value);
        }
    }

    public class DynamicStateMachine<TObject, TKey, TValue> : AbstractStateMachine<TObject, TKey, TValue>
    {
        public Action<TKey, TValue> Add;

        public Action<TKey> Remove;

        public DynamicStateMachine(TObject @object, Action<TKey, TValue> add, Action<TKey> remove)
            : base(@object)
        {
            this.Add = add;
            this.Remove = remove;
        }

        protected override void AddProviderToObject(IProvider<TKey, TValue> provider)
        {
            Add(provider.Key, provider.Value);
        }

        protected override void RemoveProviderFromObject(IProvider<TKey, TValue> provider)
        {
            Remove(provider.Key);
        }
    }
}
