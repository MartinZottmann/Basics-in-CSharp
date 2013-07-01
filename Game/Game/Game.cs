using MartinZottmann.Game.State;
using System;

namespace MartinZottmann.Game
{
    public class Game : IDisposable
    {
        protected GameState state;

        public GameState State
        {
            get { return state; }
            set
            {
                if (state != null)
                    state.Dispose();

                state = value;

                GC.Collect();
            }
        }

        public Game(GameState state)
        {
            State = state;
        }

        public void Dispose()
        {
            State.Dispose();
        }

        public void Update(double delta_time)
        {
            State.Update(delta_time);
        }

        public void Render(double delta_time)
        {
            State.Render(delta_time);
        }
    }
}
