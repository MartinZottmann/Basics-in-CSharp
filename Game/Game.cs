using MartinZottmann.Game.State;
using System;

namespace MartinZottmann.Game
{
    public class Game
    {
        protected GameState state;

        public GameState State
        {
            get
            {
                return state;
            }
            set
            {
                if (null == state)
                {
                    state = value;
                }
                else
                {
                    Stop();
                    state = value;
                    Start();
                }
            }
        }

        public Game() { }

        public void Start()
        {
            State.Start();
        }

        public void Update(double delta_time)
        {
            State.Update(delta_time);
        }

        public void Render(double delta_time)
        {
            State.Render(delta_time);
        }

        public void Stop()
        {
            State.Stop();
        }
    }
}
