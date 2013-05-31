using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartinZottmann
{
    public class Game
    {
        private List<Entity> entities = new List<Entity>();

        public Size ClientSize { get; set; }

        public Game(Size ClientSize)
        {
            this.ClientSize = ClientSize;

            for (int i = 1; i <= 100; i++)
            {
                entities.Add(new SuperBall(ClientSize.Width / 2.0f, ClientSize.Height / 2.0f));
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

        public void Draw(double delta_time, Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb(15, 0, 0, 0)), 0, 0, ClientSize.Width, ClientSize.Height);
            //g.Clear(Color.FromArgb(15, 0, 0, 0));
            foreach (Entity entity in entities)
            {
                entity.Draw(delta_time, g);
            }
        }
    }
}
