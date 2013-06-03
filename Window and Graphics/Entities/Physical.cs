using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Entities
{
    public class Physical : Entity
    {
        public Vector2d Velocity = Vector2d.Zero;

        public double Angle = 0;

        public double AngleVelocity = 10;

        public override void Update(double delta_time)
        {

            Angle += AngleVelocity * delta_time;
            if (Angle > 359)
            {
                Angle -= 360;
            }
            else if (Angle < 0)
            {
                Angle += 360;
            }

            Position += Velocity * delta_time;
            Velocity += new Vector2d(
                (randomNumber.NextDouble() - 0.5) * 100.0 * delta_time,
                (randomNumber.NextDouble() - 0.5) * 100.0 * delta_time
            );
        }

#if DEBUG
        public override void Render(double delta_time)
        {
            base.Render(delta_time);
            RenderVelocity(delta_time);
        }

        public virtual void RenderVelocity(double delta_time) {
            GL.PushMatrix();
            GL.PointSize(1);
            GL.Color4(color.R, color.G, color.B, (byte)127);
            GL.Translate(Position.X, Position.Y, 0);
            GL.Begin(BeginMode.Lines);
            GL.Vertex2(0, 0);
            GL.Vertex2(Velocity);
            GL.End();
            GL.PopMatrix();
        }
#endif
    }
}
