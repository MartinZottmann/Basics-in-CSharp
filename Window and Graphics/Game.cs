using MartinZottmann.Entities;
using MartinZottmann.Math;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MartinZottmann
{
    public class Game
    {
        protected List<Entity> entities = new List<Entity>();

        public Size ClientSize { get; set; }

        protected Physical steerable;

        public MouseDevice Mouse { get; set; }

        public KeyboardDevice Keyboard { get; set; }

        public Game(Size ClientSize)
        {
            this.ClientSize = ClientSize;

            var asteroid = new Asteroid();
            asteroid.Position.X = ClientSize.Width / 2.0f;
            asteroid.Position.Y = ClientSize.Height / 2.0f;
            asteroid.Polygon = new Polygon(
                new Vector2d[] {
                    new Vector2d(-10, 0),
                    new Vector2d(0, 10),
                    new Vector2d(10, -10)
                }
            );
            entities.Add(asteroid);

            var textured = new Textured();
            steerable = textured;
            textured.Position.X = ClientSize.Width / 2.0f;
            textured.Position.Y = ClientSize.Height / 2.0f;
            textured.quad[0] = new Vector2d(-25, -25);
            textured.quad[1] = new Vector2d(-25, 25);
            textured.quad[2] = new Vector2d(25, 25);
            textured.quad[3] = new Vector2d(25, -25);
            entities.Add(textured);

            for (int i = 1; i <= 100; i++)
            {
                var super_ball = new SuperBall();
                super_ball.Position.X = ClientSize.Width / 2.0f;
                super_ball.Position.Y = ClientSize.Height / 2.0f;
                entities.Add(super_ball);
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
            //foreach (Entity e1 in entities)
            //{
            //    foreach (Entity e2 in entities)
            //    {
            //        if (e1 != e2 && e1 is SuperBall)
            //        {
            //            float distance = e1.Position.Distance(e2.Position);
            //            Vector2 direction = (e2.Position - e1.Position).Normalize();
            //            (e1 as SuperBall).Velocity += direction * (float)((distance / 10000.0) * (delta_time / 10000.0));
            //        }
            //    }
            //}

            foreach (Entity entity in entities)
            {
                entity.Update(delta_time);

                entity.Reposition(ClientSize.Width, ClientSize.Height);
            }
        }

        public void Render(double delta_time)
        {
            foreach (Entity entity in entities)
            {
                entity.Render(delta_time);
            }
        }
    }
}
