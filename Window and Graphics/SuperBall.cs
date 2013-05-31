using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartinZottmann
{
    class SuperBall : Entity
    {
        public Pen Pen { get; set; }

        protected static Random randomNumber = new Random();

        public Vector2 Velocity { get; set; }

        public SuperBall(Vector2 Position, Vector2 Velocity)
            : base(Position)
        {
            this.Velocity = Velocity;

            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomNumber.Next(names.Length)];
            Color color = Color.FromKnownColor(randomColorName);
            //color = Color.FromArgb(31, color);
            Pen = new Pen(color);
        }

        public SuperBall(float Position_X, float Position_Y)
            : this(new Vector2(Position_X, Position_Y), new Vector2(0, 0))
        {
        }

        public SuperBall(float Position_X, float Position_Y, float Velocity_X, float Velocity_Y)
            : base(Position_X, Position_Y)
        {
            Position.X = Position_X;
            Position.Y = Position_Y;
            Velocity = new Vector2(Velocity_X, Velocity_Y);
        }

        public override void Update(double delta_time)
        {
            Position += Velocity * (float)delta_time;
            Velocity += new Vector2(
                (float)((randomNumber.NextDouble() - 0.5) * (float)delta_time / 10000.0),
                (float)((randomNumber.NextDouble() - 0.5) * (float)delta_time / 10000.0)
            );
        }

        public override void Draw(double delta_time, Graphics g)
        {
            g.DrawEllipse(Pen, Position.X - 1, Position.Y - 1, 3, 3);
        }
    }
}
