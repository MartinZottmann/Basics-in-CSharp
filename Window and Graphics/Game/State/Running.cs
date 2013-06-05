using MartinZottmann.Entities;
using MartinZottmann.Math;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Collections.Generic;
using System.Drawing;

namespace MartinZottmann.Game.State
{
    class Running : GameState
    {
        protected List<Entity> entities = new List<Entity>();

        protected Entities.GUI.FPSCounter fps_counter = new Entities.GUI.FPSCounter();

        protected Physical steerable;

        protected Camera camera;

        public Running(GameWindow window) : base(window) { }

        public override void Load()
        {
            camera = new Camera(Window);
            camera.Position.X = 100;
            camera.Position.Y = 10;
            camera.Position.Z = 100;
            camera.LookAt.X = -1;
            camera.LookAt.Z = -1;

            entities.Add(new Entities.GUI.FPSCounter());

            entities.Add(new Grid());

            var asteroid = new Asteroid();
            asteroid.Polygon = new Polygon(
                new Vector3d[] {
                    new Vector3d(-5, 0, 0),
                    new Vector3d(0, 0, 5),
                    new Vector3d(5, 0, -5)
                }
            );
            entities.Add(asteroid);

            var textured = new Textured();
            steerable = textured;
            textured.quad[0] = new Vector3d(-10, 0, -10);
            textured.quad[1] = new Vector3d(-10, 0, 10);
            textured.quad[2] = new Vector3d(10, 0, 10);
            textured.quad[3] = new Vector3d(10, 0, -10);
            entities.Add(textured);

            for (int i = 1; i <= 10; i++)
            {
                entities.Add(new SuperBall());
            }

            System.Windows.Forms.Cursor.Hide();

            Window.Mouse.Move += new System.EventHandler<MouseMoveEventArgs>(UpdateCamera);
        }

        public override void Unload()
        {
            System.Windows.Forms.Cursor.Show();

            entities.Clear();
        }

        int mouse_x_delta;

        int mouse_y_delta;

        private void UpdateCamera(object sender, MouseMoveEventArgs e)
        {
            mouse_x_delta = e.XDelta;
            mouse_y_delta = e.YDelta;
            System.Windows.Forms.Cursor.Position = Window.PointToScreen(
                new System.Drawing.Point(
                    (Window.ClientRectangle.Left + Window.ClientRectangle.Right) / 2,
                    (Window.ClientRectangle.Top + Window.ClientRectangle.Bottom) / 2
                )
            );
        }

        public override void Update(double delta_time)
        {
            if (Window.Keyboard[Key.W])
            {
                steerable.Velocity.Y += 100 * delta_time;
            }
            if (Window.Keyboard[Key.S])
            {
                steerable.Velocity.Y -= 100 * delta_time;
            }
            if (Window.Keyboard[Key.A])
            {
                steerable.Velocity.X -= 100 * delta_time;
            }
            if (Window.Keyboard[Key.D])
            {
                steerable.Velocity.X += 100 * delta_time;
            }
            if (Window.Keyboard[Key.F])
            {
                camera.RotateLookAtAroundUp(delta_time);
            }
            if (Window.Keyboard[Key.H])
            {
                camera.RotateLookAtAroundUp(-delta_time);
            }
            if (Window.Keyboard[Key.T])
            {
                camera.RotateLookAtAroundRight(delta_time);
            }
            if (Window.Keyboard[Key.G])
            {
                camera.RotateLookAtAroundRight(-delta_time);
            }
            if (mouse_x_delta != 0)
            {
                camera.RotateLookAtAroundUp(mouse_x_delta * delta_time * 0.1);
                mouse_x_delta = 0;
            }
            if (mouse_y_delta != 0)
            {
                camera.RotateLookAtAroundRight(mouse_y_delta * delta_time * 0.1);
                mouse_y_delta = 0;
            }

            foreach (Entity entity in entities)
            {
                entity.Update(delta_time);

                entity.Reposition(100, 100, 100);
            }

            fps_counter.Update(delta_time);
        }

        public override void Render(double delta_time)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            #region 3D
            GL.PushMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            var projection_matrix = camera.ProjectionMatrix();
            GL.LoadMatrix(ref projection_matrix);

            GL.PushMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
            var modelview_matrix = camera.ModelviewMatrix();
            GL.LoadMatrix(ref modelview_matrix);

            foreach (Entity entity in entities)
            {
                entity.Render(delta_time);
            }

            GL.PopMatrix();
            GL.PopMatrix();
            #endregion

            #region 2D
            GL.PushMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Window.Width, 0, Window.Height, -1, 1);
            GL.Viewport(0, 0, Window.Width, Window.Height);
            GL.PushMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            fps_counter.Render(delta_time);

            GL.PopMatrix();
            GL.PopMatrix();
            #endregion

            Window.SwapBuffers();
        }
    }
}
