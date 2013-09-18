using System;
using System.Collections.Generic;

namespace MartinZottmann.Engine.States
{
    [Serializable]
    public class StateMachine<TObject, TValue>
    {
        public TObject @object;

        public Action<TValue> Add;

        public Action<TValue> Remove;

        protected Dictionary<string, State<TValue>> states = new Dictionary<string, State<TValue>>();

        protected State<TValue> current_state;

        public StateMachine(TObject @object, Action<TValue> add, Action<TValue> remove)
        {
            this.@object = @object;
            this.Add = add;
            this.Remove = remove;
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
                        Remove(old_provider.Value);

            foreach (var new_provider in new_providers)
                Add(new_provider.Value);

            current_state = state;
        }
    }

    [Serializable]
    public class DynamicStateMachine<TObject, TKey, TValue>
    {
        public TObject @object;

        public Action<TKey, TValue> Add;

        public Action<TKey> Remove;

        protected Dictionary<string, State<TKey, TValue>> states = new Dictionary<string, State<TKey, TValue>>();

        protected State<TKey, TValue> current_state;

        public DynamicStateMachine(TObject @object, Action<TKey, TValue> add, Action<TKey> remove)
        {
            this.@object = @object;
            this.Add = add;
            this.Remove = remove;
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
                        Remove(old_provider.Key);

            foreach (var new_provider in new_providers)
                Add(new_provider.Key, new_provider.Value);

            current_state = state;
        }
    }
}
