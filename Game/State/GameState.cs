using OpenTK;
using System;

namespace MartinZottmann.Game.State
{
    public abstract class GameState
    {
        protected Window Window { get; set; }

        public GameState(Window window)
        {
            Window = window;
        }

        public abstract void Start();

        public abstract void Update(double delta_time);

        public abstract void Render(double delta_time);

        public abstract void Stop();
    }
}
