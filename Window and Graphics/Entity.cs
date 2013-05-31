using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartinZottmann
{
    public class Entity
    {
        public Vector2 Position { get; set; }

        public Entity(Vector2 Position)
        {
            this.Position = Position;
        }

        public Entity(float X, float Y)
            : this(new Vector2(X, Y))
        {
        }

        public virtual void Reposition(float max_x, float max_y)
        {

            if (Position.X < 0)
            {
                Position.X = max_x;
            }
            else if (Position.X > max_x)
            {
                Position.X = 0;
            }

            if (Position.Y < 0)
            {
                Position.Y = max_y;
            }
            else if (Position.Y > max_y)
            {
                Position.Y = 0;
            }
        }

        public virtual void Update(double delta_time)
        {
        }

        public virtual void Draw(double delta_time, Graphics g)
        {
            g.DrawEllipse(Pens.White, Position.X - 1, Position.Y - 1, 3, 3);
        }
    }
}
