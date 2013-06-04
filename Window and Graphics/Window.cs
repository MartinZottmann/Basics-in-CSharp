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
        public Game game;

        public Window(GraphicsMode mode) : base(800, 600, mode, "Test") { }

        public Thread game_thread;

        protected Entities.GUI.FPSCounter fps_counter = new Entities.GUI.FPSCounter();

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
            //GL.Disable(EnableCap.CullFace);
            //GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Lighting);

            Keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(OnKeyUp);

            // Subscribe to mouse events
            game = new Game();
            game.Mouse = Mouse;
            game.Keyboard = Keyboard;

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
                Update(update_time.Elapsed.TotalSeconds);
                update_time.Reset();
                update_time.Start();

                Render(render_time.Elapsed.TotalSeconds);
                render_time.Reset();
                render_time.Start();
            }

            Context.MakeCurrent(null);
        }

        protected static Random randomNumber = new Random();

        protected Vector3d camera_position = new Vector3d(100, 100, 100);

        protected Vector3d camera_velocity = Vector3d.Zero;
        
        protected void Update(double delta_time)
        {
            camera_position += camera_velocity * delta_time;
            camera_velocity += new Vector3d(
                (randomNumber.NextDouble() - 0.5) * 10.0 * delta_time,
                (randomNumber.NextDouble() - 0.5) * 10.0 * delta_time,
                (randomNumber.NextDouble() - 0.5) * 10.0 * delta_time
            );
            camera_velocity += (Vector3d.Zero - camera_position) * delta_time / 1000;

            fps_counter.Update(delta_time);

            game.Update(delta_time);
        }

        protected void Render(double delta_time)
        {
            #region 3D
            GL.PushMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 projection_matrix;
            projection_matrix = OpenTK.Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Width / (float)Height, 0.1f, 1000.0f);
            GL.LoadMatrix(ref projection_matrix);

            GL.PushMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
            Matrix4 modelview_matrix = Matrix4.LookAt((Vector3)camera_position, Vector3.Zero, Vector3.UnitY);
            GL.LoadMatrix(ref modelview_matrix);

            game.Render(delta_time);

            GL.PopMatrix();
            GL.PopMatrix();
            #endregion

            #region 2D
            GL.PushMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Width, 0, Height, -1, 1);
            GL.Viewport(0, 0, Width, Height);
            GL.PushMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            fps_counter.Render(delta_time);

            GL.PopMatrix();
            GL.PopMatrix();
            #endregion

            SwapBuffers();
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
