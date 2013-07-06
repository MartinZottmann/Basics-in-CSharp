using OpenTK.Graphics;
using System;

namespace MartinZottmann.Game
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (Window game = new Window(new GraphicsMode(new ColorFormat(32), 24, 0, 2, new ColorFormat(32))))
                game.Run();
        }
    }
}
