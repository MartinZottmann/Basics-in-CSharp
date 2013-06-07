using MartinZottmann.Entities;
using MartinZottmann.Game.State;
using MartinZottmann.Math;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;

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
#if DEBUG
            MartinZottmann.Program.OpenGLDebug();
#endif
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
