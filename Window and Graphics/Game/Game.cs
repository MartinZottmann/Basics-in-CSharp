using MartinZottmann.Game.State;
using OpenTK;

namespace MartinZottmann.Game
{
    public class Game
    {
        GameState state;

        GameWindow Window { get; set; }

        public Game(GameWindow window)
        {
            Window = window;

            state = new Running(window);
            state.Load();
        }

        public void Update(double delta_time)
        {
            state.Update(delta_time);
        }

        public void Render(double delta_time)
        {
            state.Render(delta_time);
        }
    }
}
