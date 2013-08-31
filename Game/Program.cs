using OpenTK.Graphics;
using System;
using System.Diagnostics;

namespace MartinZottmann.Game
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
#if DEBUG
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
#endif
            using (Window game = new Window(new GraphicsMode(new ColorFormat(32), 24, 8, 4, new ColorFormat(32))))
                game.Run();
        }
    }
}
