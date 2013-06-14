using OpenTK;
using System;

namespace MartinZottmann.Game.State
{
    public abstract class GameState : IDisposable
    {
        protected GameWindow Window { get; set; }

        public GameState(GameWindow window)
        {
            Window = window;
        }

        public abstract void Dispose();

        public abstract void Update(double delta_time);

        public abstract void Render(double delta_time);
    }
}
