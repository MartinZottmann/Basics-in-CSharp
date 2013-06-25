using MartinZottmann.Game.Entities;
using MartinZottmann.Game.Entities.Helper;
using OpenTK;

namespace MartinZottmann.Game.AI
{
    public class Steering : Base
    {
        public Steering(Entity entity) : base(entity) { }

        public override void Update(double delta_time)
        {
            if (!(Entity is Physical))
                return;

            var entity = Entity as Physical;

            var direction = entity.Target - entity.Position;
            var distance = direction.Length;
            if (distance < 0.1)
            {
                entity.AngularVelocity *= delta_time;
                entity.Velocity *= delta_time;
            }
            else
            {
                var cos = Vector3d.Dot(entity.ForwardRelative, direction) / entity.ForwardRelative.Length / distance;
                if (!System.Double.IsNaN(cos))
                {
                    var angular_velocity_y = new Vector3d(0, entity.AngularVelocity.Y, 0);
                    angular_velocity_y = Vector3d.Cross(angular_velocity_y, entity.Forward);
                    Vector3d.Transform(ref angular_velocity_y, ref entity.Orientation, out angular_velocity_y);

                    var cross = Vector3d.Cross(entity.ForwardRelative, direction);
                    if (cross.Y > 0) // Target is right
                    {
                        entity.AddForceRelative(entity.Forward, -Vector3d.UnitX * entity.thrust * delta_time);
                        entity.AddForceRelative(-entity.Forward, Vector3d.UnitX * entity.thrust * delta_time);
                    }
                    else
                    {
                        entity.AddForceRelative(entity.Forward, Vector3d.UnitX * entity.thrust * delta_time);
                        entity.AddForceRelative(-entity.Forward, -Vector3d.UnitX * entity.thrust * delta_time);
                    }
                    entity.AngularVelocity *= System.Math.Pow(0.75, delta_time);

                    var a = entity.ForwardRelative * entity.thrust;
                    var v0 = entity.Velocity;
                    var s = entity.Position - entity.Target;
                    var x = v0.Length / a.Length * 1000;
                    var y = 2 * s.Length / v0.Length;

                    if (cos > 0.99)
                        if (x <= y)
                            entity.AddForceRelative(-entity.Forward, entity.Forward * entity.thrust * delta_time);
                        else
                            entity.AddForceRelative(entity.Forward, -entity.Forward * entity.thrust * delta_time);
                    if (cos < -0.99)
                        entity.AddForceRelative(entity.Forward, -entity.Forward * entity.thrust * delta_time);
                }
            }
        }
    }
}
