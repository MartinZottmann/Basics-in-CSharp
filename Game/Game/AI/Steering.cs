using MartinZottmann.Game.Entities;
using OpenTK;

namespace MartinZottmann.Game.AI
{
    public struct SteeringCommand
    {
        public Vector3d Force;

        public Vector3d Torque;
    }

    public class Steering : Base
    {
        public double DistanceEpsilon = 0.1;

        public double DistanceEpsilonSquared = 0.01;

        public double RotationEpsilon = 0.0001;

        public Steering(Entity entity) : base(entity) { }

        public override void Update(double delta_time)
        {
            if (!(Entity is Physical))
                return;
            var entity = Entity as Physical;

            Steer(TurnToVelocity(Arrive(entity.Target, delta_time), delta_time), delta_time);
        }

        protected void Steer(SteeringCommand steering_command, double delta_time)
        {
            if (!(Entity is Physical))
                return;
            var entity = Entity as Physical;

            if (steering_command.Force.LengthSquared > DistanceEpsilonSquared)
                entity.Force += steering_command.Force - entity.Velocity;

            if (steering_command.Torque.LengthSquared > DistanceEpsilonSquared)
                entity.Torque += steering_command.Torque - entity.AngularVelocity;
        }

        protected SteeringCommand TurnToVelocity(SteeringCommand steering_command, double delta_time)
        {
            if (!(Entity is Physical))
                return steering_command;
            var entity = Entity as Physical;
            if (entity.Velocity.LengthSquared <= DistanceEpsilonSquared)
                return steering_command;

            var max_torque = 1;
            var target = entity.Velocity;
            var actual = entity.ForwardRelative;
            var direction = Vector3d.Cross(actual, target);
            var distance = 1 - Vector3d.Dot(target, actual) / target.Length / actual.Length;
            if (distance > RotationEpsilon)
            {
                var deceleration = entity.AngularVelocity.Length;
                var velocity_0 = entity.AngularVelocity.Length;
                var slowing_distance = System.Math.Pow(deceleration, 2) / 2 + velocity_0 * delta_time; // / max_torque * entity.Inertia; // slowing_distance = decelertation^2/2 + velocity_0 * delta_time
                if (distance > slowing_distance)
                    steering_command.Torque = direction.Normalized() * max_torque;
                else
                    steering_command.Torque = -1 * entity.AngularVelocity.Normalized() * max_torque;
            }

            return steering_command;
        }

        protected Vector3d Seek(Vector3d target, double delta_time)
        {
            if (!(Entity is Physical))
                return Vector3d.Zero;
            var entity = Entity as Physical;

            return target - entity.Position;
        }

        protected SteeringCommand Arrive(Vector3d target, double delta_time)
        {
            var steering_command = new SteeringCommand();

            if (!(Entity is Physical))
                return steering_command;
            var entity = Entity as Physical;
            var max_force = entity.thrust;
            var direction = Seek(target, delta_time) - entity.Velocity - entity.Velocity * delta_time;
            var distance = direction.Length;

            if (distance > DistanceEpsilon)
            {
                var deceleration = entity.Velocity.Length;
                var velocity_0 = entity.Velocity.Length;
                var slowing_distance = System.Math.Pow(deceleration, 2) / 2 + velocity_0 * delta_time; // / max_force * entity.Mass; // slowing_distance = decelertation^2/2 + velocity_0 * delta_time
                if (distance > slowing_distance)
                    steering_command.Force = direction / distance * max_force;
                else
                    steering_command.Force = -1 * entity.Velocity.Normalized() * max_force;
            }

            return steering_command;
        }
    }
}
