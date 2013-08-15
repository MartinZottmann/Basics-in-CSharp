using MartinZottmann.Game.Entities;
using MartinZottmann.Game.Entities.Components;
using OpenTK;

namespace MartinZottmann.Game.AI
{
    public struct SteeringCommand
    {
        public Vector3d Force;

        public Vector3d Torque;
    }

    public class Steering<T> : Base<T> where T : GameObject
    {
        public double DistanceEpsilon = 0.1;

        public double DistanceEpsilonSquared = 0.01;

        public double RotationEpsilon = 0.0001;

        public Steering(T subject) : base(subject) { }

        public override void Update(double delta_time)
        {
            if (Subject.GetComponent<Target>().Position.HasValue)
                Steer(TurnToVelocity(Arrive(Subject.GetComponent<Target>().Position.Value, delta_time), delta_time), delta_time);
        }

        protected void Steer(SteeringCommand steering_command, double delta_time)
        {
            var physic = Subject.GetComponent<Physic>();

            if (steering_command.Force.LengthSquared > DistanceEpsilonSquared)
                physic.Force += steering_command.Force - physic.Velocity;

            if (steering_command.Torque.LengthSquared > DistanceEpsilonSquared)
                physic.Torque += steering_command.Torque - physic.AngularVelocity;
        }

        protected SteeringCommand TurnToVelocity(SteeringCommand steering_command, double delta_time)
        {
            var physic = Subject.GetComponent<Physic>();

            if (physic.Velocity.LengthSquared <= DistanceEpsilonSquared)
                return steering_command;

            var max_torque = 1;
            var target = physic.Velocity;
            var actual = Subject.ForwardRelative;
            var direction = Vector3d.Cross(actual, target);
            var distance = 1 - Vector3d.Dot(target, actual) / target.Length / actual.Length;
            if (distance > RotationEpsilon)
            {
                var deceleration = physic.AngularVelocity.Length;
                var velocity_0 = physic.AngularVelocity.Length;
                var slowing_distance = System.Math.Pow(deceleration, 2) / 2 + velocity_0 * delta_time; // / max_torque * entity.Inertia; // slowing_distance = decelertation^2/2 + velocity_0 * delta_time
                if (distance > slowing_distance)
                    steering_command.Torque = direction.Normalized() * max_torque;
                else
                    steering_command.Torque = -1 * physic.AngularVelocity.Normalized() * max_torque;
            }

            return steering_command;
        }

        protected Vector3d Seek(Vector3d target, double delta_time)
        {
            return target - Subject.Position;
        }

        protected SteeringCommand Arrive(Vector3d target, double delta_time)
        {
            var physic = Subject.GetComponent<Physic>();

            var steering_command = new SteeringCommand();
            var max_force = physic.thrust;
            var direction = Seek(target, delta_time) - physic.Velocity - physic.Velocity * delta_time;
            var distance = direction.Length;

            if (distance > DistanceEpsilon)
            {
                var deceleration = physic.Velocity.Length;
                var velocity_0 = physic.Velocity.Length;
                var slowing_distance = System.Math.Pow(deceleration, 2) / 2 + velocity_0 * delta_time; // / max_force * entity.Mass; // slowing_distance = decelertation^2/2 + velocity_0 * delta_time
                if (distance > slowing_distance)
                    steering_command.Force = direction / distance * max_force;
                else
                    steering_command.Force = -1 * physic.Velocity.Normalized() * max_force;
            }

            return steering_command;
        }
    }
}
