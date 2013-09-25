using MartinZottmann.Game.State;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace MartinZottmann.Game
{
    public class Window : GameWindow
    {
        public Game game;

        public Window(GraphicsMode mode) : base(800, 600, mode, "Codename Void", GameWindowFlags.Default, DisplayDevice.Default, 4, 3, GraphicsContextFlags.Default) { }

        public Thread game_thread;

        protected bool request_context = false;

        protected Object request_context_lock = new Object();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            VSync = VSyncMode.Off;

            Console.WriteLine("Vendor: {0}", GL.GetString(StringName.Vendor));
            Console.WriteLine("Renderer: {0}", GL.GetString(StringName.Renderer));
            Console.WriteLine("Version: {0}", GL.GetString(StringName.Version));
            Console.WriteLine("GLSL: {0}", GL.GetString(StringName.ShadingLanguageVersion));
            int info;
            GL.GetInteger(GetPName.NumExtensions, out info);
            Console.WriteLine("{0}: {1}", GetPName.NumExtensions, info);
            GL.GetInteger(GetPName.Multisample, out info);
            Console.WriteLine("{0}: {1}", GetPName.Multisample, info);
            GL.GetInteger(GetPName.Samples, out info);
            Console.WriteLine("{0}: {1}", GetPName.Samples, info);

            // Initialize OpenGL properties
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.ClearColor(System.Drawing.Color.Black);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.Hint(HintTarget.FragmentShaderDerivativeHint, HintMode.Nicest);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
            GL.Hint(HintTarget.TextureCompressionHint, HintMode.Nicest);

            Keyboard.KeyUp += OnKeyUp;

            // Subscribe to mouse events
            game = new Game();

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
            const double update_min_dt = 10.0 / 1000.0;
            const double update_max_dt = 30.0 / 1000.0;
            var update_acc_dt = update_max_dt;
            const double render_min_dt = 30.0 / 1000.0;
            const double render_max_dt = 1000.0 / 1000.0;
            var render_acc_dt = render_max_dt;
            Stopwatch update_time = new Stopwatch();
            Stopwatch render_time = new Stopwatch();

            MakeCurrent();

            game.State = new Running(this);
            game.Start();

            update_time.Start();
            render_time.Start();

            while (Exists && !IsExiting)
            {
                if (request_context)
                {
                    Context.MakeCurrent(null);
                    lock (request_context_lock)
                    {
                        Monitor.Pulse(request_context_lock);
                        Monitor.Wait(request_context_lock);
                    }
                    MakeCurrent();
                }

                var update_cur_dt = update_time.Elapsed.TotalSeconds;
                update_time.Restart();
                update_acc_dt += update_cur_dt;
                if (update_acc_dt <= update_min_dt)
                {
                    // Skip update
                }
                else if (update_acc_dt >= update_max_dt)
                {
                    game.Update(update_max_dt);
                    update_acc_dt -= update_max_dt;
                }
                else
                {
                    game.Update(update_max_dt);
                    update_acc_dt -= update_max_dt;
                }

                var render_cur_dt = render_time.Elapsed.TotalSeconds;
                render_time.Restart();
                render_acc_dt += render_cur_dt;
                if (render_acc_dt <= render_min_dt)
                {
                    // Skip render
                }
                else if (render_acc_dt >= render_max_dt)
                {
                    game.Render(render_max_dt);
                    render_acc_dt -= render_max_dt;
                }
                else
                {
                    game.Render(render_cur_dt);
                    render_acc_dt -= render_cur_dt;
                }
            }

            Context.MakeCurrent(null);

            game.Stop();
        }

        public void RequestContext()
        {
            request_context = true;

            lock (request_context_lock)
                Monitor.Wait(request_context_lock);
            MakeCurrent();
        }

        public void ReleaseContext()
        {
            request_context = false;

            Context.MakeCurrent(null);
            lock (request_context_lock)
                Monitor.Pulse(request_context_lock);
        }

        protected void OnKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Exit();

            if (e.Key == Key.F11)
                if (WindowState != WindowState.Fullscreen)
                    WindowState = WindowState.Fullscreen;
                else
                    WindowState = WindowState.Normal;

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
            RequestContext();
            GL.ReadPixels(0, 0, ClientSize.Width, ClientSize.Height, PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
            ReleaseContext();
            bmp.UnlockBits(data);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            return bmp;
        }
    }
}
