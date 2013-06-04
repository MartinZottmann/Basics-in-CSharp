using MartinZottmann.Entities;
using MartinZottmann.Math;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MartinZottmann
{
    public class Game
    {
        protected List<Entity> entities = new List<Entity>();

        protected Physical steerable;

        public MouseDevice Mouse { get; set; }

        public KeyboardDevice Keyboard { get; set; }

        public Game()
        {
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
        }

        public void Update(double delta_time)
        {
            if (Keyboard[Key.W])
            {
                steerable.Velocity.Y += 100 * delta_time;
            }
            if (Keyboard[Key.S])
            {
                steerable.Velocity.Y -= 100 * delta_time;
            }
            if (Keyboard[Key.A])
            {
                steerable.Velocity.X -= 100 * delta_time;
            }
            if (Keyboard[Key.D])
            {
                steerable.Velocity.X += 100 * delta_time;
            }

            foreach (Entity entity in entities)
            {
                entity.Update(delta_time);

                entity.Reposition(100, 100, 100);
            }
        }

        public void Render(double delta_time)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //camera.Render(delta_time);

            foreach (Entity entity in entities)
            {
                entity.Render(delta_time);
            }
        }
    }
}
