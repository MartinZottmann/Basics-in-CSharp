namespace MartinZottmann.Engine.States
{
    public interface IStateMachine<TObject, TValue> : IStatable<IStateMachine<TObject, TValue>, string, State<TValue>>
    {
        State<TValue> CreateState(string name);

        void AddState(string name, State<TValue> state);

        void RemoveState(string name);

        void ChangeState(string name);
    }

    public interface IStateMachine<TObject, TKey, TValue> : IStatable<IStateMachine<TObject, TKey, TValue>, string, State<TKey, TValue>>
    {
        State<TKey, TValue> CreateState(string name);

        void AddState(string name, State<TKey, TValue> state);

        void RemoveState(string name);

        void ChangeState(string name);
    }
}
