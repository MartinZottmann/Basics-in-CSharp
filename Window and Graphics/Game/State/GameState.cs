using OpenTK;
using OpenTK.Input;
using System.Drawing;

namespace MartinZottmann.Game.State
{
    abstract class GameState
    {
        protected GameWindow Window { get; set; }

        public GameState(GameWindow window)
        {
            Window = window;
        }

        public abstract void Load();

        public abstract void Unload();

        public abstract void Update(double delta_time);

        public abstract void Render(double delta_time);
    }
}
