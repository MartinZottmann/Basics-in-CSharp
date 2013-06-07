using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

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

#if DEBUG
        public static void OpenGLDebug([CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var error = GL.GetError();
            while (error != ErrorCode.NoError)
            {
                Console.Error.WriteLine("OpenGL Error: {0} at {1} in {2} at {3}", error, memberName, sourceFilePath, sourceLineNumber);
                error = GL.GetError();
            }
        }
#endif
    }
}
