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

            Console.WriteLine("Vendor: {0}", GL.GetString(StringName.Vendor));
            Console.WriteLine("Renderer: {0}", GL.GetString(StringName.Renderer));
            Console.WriteLine("Version: {0}", GL.GetString(StringName.Version));
            Console.WriteLine("GLSL: {0}", GL.GetString(StringName.ShadingLanguageVersion));
            int info;
            GL.GetInteger(GetPName.NumExtensions, out info);
            Console.WriteLine("Num. Extensions: {0}", info);

            // Initialize OpenGL properties
            GL.AlphaFunc(AlphaFunction.Greater, 0.1f);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.ClearColor(System.Drawing.Color.Black);
            //GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.PointSmooth);
            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Disable(EnableCap.Lighting);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

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
#if DEBUG
                MartinZottmann.Program.OpenGLDebug();
#endif
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
