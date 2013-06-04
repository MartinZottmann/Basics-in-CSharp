using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Entities
{
    public class Physical : Entity
    {
        public Vector3d Velocity = Vector3d.Zero;

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
            Velocity += new Vector3d(
                (randomNumber.NextDouble() - 0.5) * 10.0 * delta_time,
                (randomNumber.NextDouble() - 0.5) * 10.0 * delta_time,
                (randomNumber.NextDouble() - 0.5) * 10.0 * delta_time
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
            GL.Begin(BeginMode.Lines);
            GL.Color4(color.R, color.G, color.B, (byte)127);
            GL.Vertex3(Position);
            GL.Vertex3(Position + Velocity);
            GL.Color4(1, 1, 1, 0.2);
            GL.Vertex3(Position);
            GL.Vertex3(Position.X, 0, Position.Z);

            double R = 100;
            Vector3d c = Vector3d.Zero;
            Vector3d v = new Vector3d(Position.X, 0, Position.Z) - c;
            Vector3d a = c + v / v.Length * R;
            double cX = 0;
            double cY = 0;
            double cZ = 0;
            double vX = Position.X - cX;
            double vY = 0 - cY;
            double vZ = Position.Z - cZ;
            double magV = System.Math.Sqrt(vX * vX + vZ * vZ);
            double aX = cX + vX / magV * R;
            double aZ = cZ + vZ / magV * R;

            GL.Vertex3(Position.X, 0, Position.Z);
            GL.Vertex3(aX, 0, aZ);

            //GL.Vertex3(0, -Position.Y, 0);
            //GL.Vertex3(a);

            GL.End();
            GL.PopMatrix();
        }
#endif
    }
}
