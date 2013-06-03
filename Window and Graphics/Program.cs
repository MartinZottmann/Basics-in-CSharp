using OpenTK.Graphics;
using System;
using System.Windows.Forms;

namespace MartinZottmann
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            GraphicsMode mode = new GraphicsMode(new OpenTK.Graphics.ColorFormat(32), 24, 0, 2, new OpenTK.Graphics.ColorFormat(32));
            using (Window game = new Window(mode))
            {
                game.Run();
            }
        }
    }
}
