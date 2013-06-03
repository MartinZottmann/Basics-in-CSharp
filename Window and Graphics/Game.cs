using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MartinZottmann
{
    public class Game
    {
        private List<Entity> entities = new List<Entity>();

        public Size ClientSize { get; set; }

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
