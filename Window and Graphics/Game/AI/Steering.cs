using MartinZottmann.Game.Entities;
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
                    var acos = System.Math.Acos(cos);
                    if (!System.Double.IsNaN(acos))
                    {
                        var cross = Vector3d.Cross(entity.ForwardRelative, direction);
                        if (cross.Y > 0) // Target is right
                            entity.AngularVelocity.Y = System.Math.Acos(cos) / System.Math.PI / entity.Mass * entity.thrust * delta_time;
                        //entity.AddForceRelative(entity.Forward, -Vector3d.UnitX * entity.thrust * delta_time);
                        //entity.AddForceRelative(-entity.Forward, Vector3d.UnitX * entity.thrust * delta_time);
                        else
                            entity.AngularVelocity.Y = -System.Math.Acos(cos) / System.Math.PI / entity.Mass * entity.thrust * delta_time;
                        //entity.AddForceRelative(entity.Forward, Vector3d.UnitX * entity.thrust * delta_time);
                        //entity.AddForceRelative(-entity.Forward, -Vector3d.UnitX * entity.thrust * delta_time);
                    }

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
