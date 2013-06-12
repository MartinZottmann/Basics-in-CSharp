﻿using MartinZottmann.Entities;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Collections.Generic;

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
            camera.MouseLook = true;
            camera.Position.X = 10;
            camera.Position.Y = 10;
            camera.Position.Z = 100;

            entities.Add(new Entities.GUI.FPSCounter());

            entities.Add(new Grid());

            entities.Add(new Starfield());

            for (int i = 1; i <= 10; i++)
            {
                entities.Add(new Asteroid());
            }

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
        }

        public override void Unload()
        {
            entities.Clear();
        }

        public override void Update(double delta_time)
        {
            if (Window.Keyboard[Key.W])
            {
                camera.Position -= camera.Direction * delta_time * 100;
                //steerable.Velocity.Y += 100 * delta_time;
            }
            if (Window.Keyboard[Key.S])
            {
                camera.Position += camera.Direction * delta_time * 100;
                //steerable.Velocity.Y -= 100 * delta_time;
            }
            if (Window.Keyboard[Key.A])
            {
                camera.Position += camera.Right * delta_time * 100;
                //steerable.Velocity.X -= 100 * delta_time;
            }
            if (Window.Keyboard[Key.D])
            {
                camera.Position -= camera.Right * delta_time * 100;
                //steerable.Velocity.X += 100 * delta_time;
            }
            if (Window.Keyboard[Key.Space])
            {
                camera.Position += camera.Up * delta_time * 100;
            }
            if (Window.Keyboard[Key.ShiftLeft])
            {
                camera.Position -= camera.Up * delta_time * 100;
            }
            if (Window.Keyboard[Key.F])
            {
                camera.RotateDirectionAroundUp(delta_time);
            }
            if (Window.Keyboard[Key.H])
            {
                camera.RotateDirectionAroundUp(-delta_time);
            }
            if (Window.Keyboard[Key.T])
            {
                camera.RotateDirectionAroundRight(delta_time);
            }
            if (Window.Keyboard[Key.G])
            {
                camera.RotateDirectionAroundRight(-delta_time);
            }

            camera.Update(delta_time);

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
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            {
                var projection_matrix = camera.ProjectionMatrix();
                GL.LoadMatrix(ref projection_matrix);

                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();
                {
                    var view_matrix = camera.ViewMatrix();
                    GL.LoadMatrix(ref view_matrix);

                    foreach (Entity entity in entities)
                    {
                        entity.Projection = projection_matrix;
                        entity.Model = Matrix4d.Identity;
                        entity.View = view_matrix;
                        if (entity is Asteroid)
                        {
                            (entity as Asteroid).EyeDirection.Set(camera.Direction);
                        }
                        entity.Render(delta_time);
                    }
                }
                GL.MatrixMode(MatrixMode.Modelview);
                GL.PopMatrix();
            }
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            #endregion

            #region 2D
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            {
                GL.LoadIdentity();
                GL.Ortho(0, Window.Width, 0, Window.Height, -1, 1);
                GL.Viewport(0, 0, Window.Width, Window.Height);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();
                {
                    GL.LoadIdentity();

                    fps_counter.Render(delta_time);
                }
                GL.MatrixMode(MatrixMode.Modelview);
                GL.PopMatrix();
            }
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            #endregion

            Window.SwapBuffers();
        }
    }
}
