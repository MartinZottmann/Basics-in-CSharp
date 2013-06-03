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

        public Window(GraphicsMode mode) : base(800, 600, mode, "Test") { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            VSync = VSyncMode.On;

            // Initialize OpenGL properties
            GL.ClearColor(System.Drawing.Color.Black);
            //GL.AlphaFunc(AlphaFunction.Greater, 0.1f);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            //GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.PointSmooth);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Lighting);

            // Subscribe to mouse events
            game = new Game(ClientSize);
            game.Mouse = Mouse;
            game.Keyboard = Keyboard;
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

            if (Keyboard[Key.F11])
            {
                if (WindowState != WindowState.Fullscreen)
                {
                    WindowState = WindowState.Fullscreen;
                }
                else
                {
                    WindowState = WindowState.Normal;
                }
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
