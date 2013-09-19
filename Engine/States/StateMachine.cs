using System;

namespace MartinZottmann.Engine.States
{
    [Serializable]
    public class StateMachine<TObject, TValue> : AbstractStateMachine<TObject, TValue> where TObject : IStatable<TObject, TValue>
    {
        public StateMachine(TObject @object) : base(@object) { }

        protected override void AddProviderToObject(IProvider<TValue> provider)
        {
            @object.Add(provider.Value);
        }

        protected override void RemoveProviderFromObject(IProvider<TValue> provider)
        {
            @object.Remove(provider.Value);
        }
    }

    [Serializable]
    public class StateMachine<TObject, TKey, TValue> : AbstractStateMachine<TObject, TKey, TValue> where TObject : IStatable<TObject, TKey, TValue>
    {
        public StateMachine(TObject @object) : base(@object) { }

        protected override void AddProviderToObject(IProvider<TKey, TValue> provider)
        {
            @object.Add(provider.Key, provider.Value);
        }

        protected override void RemoveProviderFromObject(IProvider<TKey, TValue> provider)
        {
            @object.Remove(provider.Key);
        }
    }
}
