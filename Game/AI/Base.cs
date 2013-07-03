using MartinZottmann.Game.Entities;

namespace MartinZottmann.Game.AI
{
    public abstract class Base<T>
    {
        public T Subject;

        public Base(T subject)
        {
            Subject = subject;
        }

        public virtual void Update(double delta_time) { }
    }
}
