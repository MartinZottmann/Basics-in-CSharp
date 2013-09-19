using System;
using System.Collections.Generic;

namespace MartinZottmann.Engine.States
{
    [Serializable]
    abstract public class AbstractStateMachine<TObject, TValue> : IStateMachine<TObject, TValue>
    {
        public TObject @object;

        protected Dictionary<string, State<TValue>> states = new Dictionary<string, State<TValue>>();

        public int Count { get { return states.Count; } }

        protected State<TValue> current_state;

        public AbstractStateMachine(TObject @object)
        {
            this.@object = @object;
        }

        public State<TValue> CreateState(string name)
        {
            var state = new State<TValue>();
            states.Add(name, state);
            return state;
        }

        public void AddState(string name, State<TValue> state)
        {
            states.Add(name, state);
        }

        public void RemoveState(string name)
        {
            states.Remove(name);
        }

        public void ChangeState(string name)
        {
            var state = states[name];

            if (state == current_state)
                return;

            var new_providers = state.ProvidersCloneFlat();

            if (null != current_state)
                foreach (var old_provider in current_state.Providers)
                    if (new_providers.Contains(old_provider))
                        new_providers.Remove(old_provider);
                    else
                        RemoveProviderFromObject(old_provider);

            foreach (var new_provider in new_providers)
                AddProviderToObject(new_provider);

            current_state = state;
        }

        abstract protected void AddProviderToObject(IProvider<TValue> provider);

        abstract protected void RemoveProviderFromObject(IProvider<TValue> provider);

        #region IStatable<IStateMachine<TObject,TValue>,string,IProvider<TValue>> Members

        void IStatable<IStateMachine<TObject, TValue>, string, State<TValue>>.Add(string key, State<TValue> value)
        {
            AddState(key, value);
        }

        void IStatable<IStateMachine<TObject, TValue>, string, State<TValue>>.Remove(string key)
        {
            RemoveState(key);
        }

        #endregion
    }

    [Serializable]
    abstract public class AbstractStateMachine<TObject, TKey, TValue> : IStateMachine<TObject, TKey, TValue>
    {
        public TObject @object;

        protected Dictionary<string, State<TKey, TValue>> states = new Dictionary<string, State<TKey, TValue>>();

        public int Count { get { return states.Count; } }

        protected State<TKey, TValue> current_state;

        public AbstractStateMachine(TObject @object)
        {
            this.@object = @object;
        }

        public State<TKey, TValue> CreateState(string name)
        {
            var state = new State<TKey, TValue>();
            states.Add(name, state);
            return state;
        }

        public void AddState(string name, State<TKey, TValue> state)
        {
            states.Add(name, state);
        }

        public void RemoveState(string name)
        {
            states.Remove(name);
        }

        public void ChangeState(string name)
        {
            var state = states[name];

            if (state == current_state)
                return;

            var new_providers = state.ProvidersCloneFlat();

            if (null != current_state)
                foreach (var old_provider in current_state.Providers)
                    if (new_providers.Contains(old_provider))
                        new_providers.Remove(old_provider);
                    else
                        RemoveProviderFromObject(old_provider);

            foreach (var new_provider in new_providers)
                AddProviderToObject(new_provider);

            current_state = state;
        }

        abstract protected void AddProviderToObject(IProvider<TKey, TValue> provider);

        abstract protected void RemoveProviderFromObject(IProvider<TKey, TValue> provider);

        #region IStatable<IStateMachine<TObject,TKey,TValue>,string,State<TKey,TValue>> Members

        void IStatable<IStateMachine<TObject, TKey, TValue>, string, State<TKey, TValue>>.Add(string key, State<TKey, TValue> value)
        {
            AddState(key, value);
        }

        void IStatable<IStateMachine<TObject, TKey, TValue>, string, State<TKey, TValue>>.Remove(string key)
        {
            RemoveState(key);
        }

        #endregion
    }
}
