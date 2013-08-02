using MartinZottmann.Game.Entities;
using MartinZottmann.Game.Entities.Helper;
using OpenTK;

namespace MartinZottmann.Game.AI
{
    public struct SteeringCommand
    {
        public Vector3d Force;

        public Vector3d Torque;
    }

    public class Steering<T> : Base<T> where T : Physical, INavigation
    {
        public double DistanceEpsilon = 0.1;

        public double DistanceEpsilonSquared = 0.01;

        public double RotationEpsilon = 0.0001;

        public Steering(T subject) : base(subject) { }

        public override void Update(double delta_time)
        {
            Steer(TurnToVelocity(Arrive(Subject.Target, delta_time), delta_time), delta_time);
        }

        protected void Steer(SteeringCommand steering_command, double delta_time)
        {
            if (steering_command.Force.LengthSquared > DistanceEpsilonSquared)
                Subject.Physic.Force += steering_command.Force - Subject.Physic.Velocity;

            if (steering_command.Torque.LengthSquared > DistanceEpsilonSquared)
                Subject.Physic.Torque += steering_command.Torque - Subject.Physic.AngularVelocity;
        }

        protected SteeringCommand TurnToVelocity(SteeringCommand steering_command, double delta_time)
        {
            if (Subject.Physic.Velocity.LengthSquared <= DistanceEpsilonSquared)
                return steering_command;

            var max_torque = 1;
            var target = Subject.Physic.Velocity;
            var actual = Subject.ForwardRelative;
            var direction = Vector3d.Cross(actual, target);
            var distance = 1 - Vector3d.Dot(target, actual) / target.Length / actual.Length;
            if (distance > RotationEpsilon)
            {
                var deceleration = Subject.Physic.AngularVelocity.Length;
                var velocity_0 = Subject.Physic.AngularVelocity.Length;
                var slowing_distance = System.Math.Pow(deceleration, 2) / 2 + velocity_0 * delta_time; // / max_torque * entity.Inertia; // slowing_distance = decelertation^2/2 + velocity_0 * delta_time
                if (distance > slowing_distance)
                    steering_command.Torque = direction.Normalized() * max_torque;
                else
                    steering_command.Torque = -1 * Subject.Physic.AngularVelocity.Normalized() * max_torque;
            }

            return steering_command;
        }

        protected Vector3d Seek(Vector3d target, double delta_time)
        {
            return target - Subject.Position;
        }

        protected SteeringCommand Arrive(Vector3d target, double delta_time)
        {
            var steering_command = new SteeringCommand();
            var max_force = Subject.Physic.thrust;
            var direction = Seek(target, delta_time) - Subject.Physic.Velocity - Subject.Physic.Velocity * delta_time;
            var distance = direction.Length;

            if (distance > DistanceEpsilon)
            {
                var deceleration = Subject.Physic.Velocity.Length;
                var velocity_0 = Subject.Physic.Velocity.Length;
                var slowing_distance = System.Math.Pow(deceleration, 2) / 2 + velocity_0 * delta_time; // / max_force * entity.Mass; // slowing_distance = decelertation^2/2 + velocity_0 * delta_time
                if (distance > slowing_distance)
                    steering_command.Force = direction / distance * max_force;
                else
                    steering_command.Force = -1 * Subject.Physic.Velocity.Normalized() * max_force;
            }

            return steering_command;
        }
    }
}
