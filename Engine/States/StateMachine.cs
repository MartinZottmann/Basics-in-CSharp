using System;
using System.Collections.Generic;

namespace MartinZottmann.Engine.States
{
    [Serializable]
    public class StateMachine<T, U> where T : IStatable<U>
    {
        public T @object;

        protected Dictionary<string, State<U>> states = new Dictionary<string, State<U>>();

        protected State<U> current_state;

        protected internal StateMachine() { }

        public StateMachine(T @object)
        {
            this.@object = @object;
        }

        public State<U> CreateState(string name)
        {
            var state = new State<U>();
            states.Add(name, state);
            return state;
        }

        public void AddState(string name, State<U> state)
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

            var new_providers = state.Providers;

            if (null != current_state)
                foreach (var old_provider in current_state.Providers)
                    if (new_providers.ContainsKey(old_provider.Key) && new_providers[old_provider.Key] == old_provider.Value)
                        new_providers.Remove(old_provider.Key);
                    else
                        @object.Remove(old_provider.Key);

            foreach (var new_provider in new_providers)
                @object.Add(new_provider.Value.Get());

            current_state = state;
        }
    }
}
