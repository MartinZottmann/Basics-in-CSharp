using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;

namespace MartinZottmann
{
    class Window : GameWindow
    {
        public Game game;

        public Window(GraphicsMode mode)
            : base(800, 600, mode, "Test")
        {
            VSync = VSyncMode.On;
            //WindowState = WindowState.Fullscreen;

            game = new Game(this.ClientSize);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Initialize OpenGL properties
            // Subscribe to mouse events
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            game.ClientSize = ClientSize;

            // Set up OpenGL viewport
            int w = Width;
            int h = Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1);
            GL.Viewport(0, 0, w, h);

            // Set up projection/modelview matrices
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (Keyboard[Key.Escape])
            {
                Exit();
            }

            game.Update(e.Time);
#if DEBUG
            Accumulate(e.Time * 1000.0);
#endif
        }

#if DEBUG
        double accumulator = 0;

        int idleCounter = 0;

        protected void Accumulate(double milliseconds)
        {
            idleCounter++;
            accumulator += milliseconds;
            if (accumulator > 1000)
            {
                Console.WriteLine("FPS: {0:F}", idleCounter);
                idleCounter = 0;
                accumulator -= 1000;
            }
        }
#endif

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            game.Render(e.Time);
            SwapBuffers();
        }
    }
}
