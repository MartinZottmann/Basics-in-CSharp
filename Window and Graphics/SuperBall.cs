using OpenTK;

namespace MartinZottmann
{
    class SuperBall : Entity
    {
        public Vector2d Velocity = Vector2d.Zero;

        public override void Update(double delta_time)
        {
            Position += Velocity * delta_time;
            Velocity += new Vector2d(
                (randomNumber.NextDouble() - 0.5) * 100.0 * delta_time,
                (randomNumber.NextDouble() - 0.5) * 100.0 * delta_time
            );
        }
    }
}
