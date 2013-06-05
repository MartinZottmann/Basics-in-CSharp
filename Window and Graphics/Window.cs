using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace MartinZottmann
{
    class Window : GameWindow
    {
        public MartinZottmann.Game.Game game;

        public Window(GraphicsMode mode) : base(800, 600, mode, "Test") { }

        public Thread game_thread;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            VSync = VSyncMode.On;

            // Initialize OpenGL properties
            GL.ClearColor(System.Drawing.Color.Black);
            GL.AlphaFunc(AlphaFunction.Greater, 0.1f);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.PointSmooth);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Lighting);

            Keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(OnKeyUp);

            // Subscribe to mouse events
            game = new MartinZottmann.Game.Game(this);

            Context.MakeCurrent(null);

            game_thread = new Thread(Loop);
            game_thread.IsBackground = true;
            game_thread.Start();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Do nothing
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // Do nothing

            Thread.Sleep(1);
        }

        protected override void OnUnload(EventArgs e)
        {
            game_thread.Join();

            MakeCurrent();

            base.OnUnload(e);
        }

        protected void Loop()
        {
            MakeCurrent();

            Stopwatch update_time = new Stopwatch();
            update_time.Start();
            Stopwatch render_time = new Stopwatch();
            render_time.Start();

            while (!IsExiting)
            {
                game.Update(update_time.Elapsed.TotalSeconds);
                update_time.Reset();
                update_time.Start();

                game.Render(render_time.Elapsed.TotalSeconds);
                render_time.Reset();
                render_time.Start();
            }

            Context.MakeCurrent(null);
        }

        protected void OnKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Exit();
            }

            if (e.Key == Key.F11)
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

            if (e.Key == Key.F12)
            {
                string filename = String.Format("screenshot-{0}.png", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fffffff"));
                Console.WriteLine(filename);
                GrabScreenshot().Save(filename);
            }
        }

        protected Bitmap GrabScreenshot()
        {
            Bitmap bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            System.Drawing.Imaging.BitmapData data = bmp.LockBits(ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.ReadPixels(0, 0, ClientSize.Width, ClientSize.Height, PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            return bmp;
        }
    }
}
