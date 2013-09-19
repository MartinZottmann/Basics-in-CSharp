namespace MartinZottmann.Engine.States
{
    public interface IStateMachine<TObject, TValue>
    {
        State<TValue> CreateState(string name);

        void AddState(string name, State<TValue> state);

        void RemoveState(string name);

        void ChangeState(string name);
    }

    public interface IStateMachine<TObject, TKey, TValue>
    {
        State<TKey, TValue> CreateState(string name);

        void AddState(string name, State<TKey, TValue> state);

        void RemoveState(string name);

        void ChangeState(string name);
    }
}
