using MartinZottmann.Game;
using OpenTK.Graphics;
using System;

namespace MartinZottmann
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            GraphicsMode mode = new GraphicsMode(new OpenTK.Graphics.ColorFormat(32), 24, 0, 2, new OpenTK.Graphics.ColorFormat(32));
            using (Window game = new Window(mode))
            {
                game.Run();
            }
#if DEBUG
            Console.ReadKey();
#endif
        }
    }
}
