using MartinZottmann.Engine.Entities;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Entities.Nodes;
using OpenTK;
using System;

namespace MartinZottmann.Game.Entities.Systems
{
    public class AISystem : ISystem
    {
        public static Random Random = new Random();

        public const double DistanceEpsilon = 0.1;

        public const double DistanceEpsilonSquared = 0.01;

        public const double RotationEpsilon = 0.0001;

        protected EntityManager entity_manager;

        protected NodeList<AINode> ai_nodes;

        public void Start(EntityManager entity_manager)
        {
            this.entity_manager = entity_manager;
            ai_nodes = this.entity_manager.GetNodeList<AINode>();
        }

        public void Update(double delta_time)
        {
            foreach (var ai_node in ai_nodes)
            {
                PickTarget(ai_node);
                ProcessSteering(delta_time, ai_node);
            }
        }

        public void Render(double delta_time) { }

        public void Stop()
        {
            entity_manager = null;
            ai_nodes = null;
        }

        protected void PickTarget(AINode ai_node)
        {
            if (null != ai_node.AI.TargetPosition)
                return;

            ai_node.AI.TargetPosition = new Vector3d(
                (Random.NextDouble() - 0.5) * 100.0,
                (Random.NextDouble() - 0.5) * 100.0,
                (Random.NextDouble() - 0.5) * 100.0
            );
        }

        protected void ProcessSteering(double delta_time, AINode ai_node)
        {
            if (null == ai_node.AI.TargetPosition)
                return;
            
            ProcessArrival(delta_time, ai_node);
            ProcessTurnToVelocity( delta_time,  ai_node);
        }

        protected void ProcessArrival(double delta_time, AINode ai_node)
        {
            var physic = ai_node.Entity.Get<PhysicComponent>();

            var max_force = physic.thrust;
            var direction = ai_node.AI.TargetPosition.Value - ai_node.Base.Position - physic.Velocity - physic.Velocity * delta_time;
            var distance = direction.Length;

            if (distance > DistanceEpsilon)
            {
                var deceleration = physic.Velocity.Length;
                var velocity_0 = physic.Velocity.Length;
                var slowing_distance = System.Math.Pow(deceleration, 2) / 2 + velocity_0 * delta_time; // / max_force * entity.Mass; // slowing_distance = decelertation^2/2 + velocity_0 * delta_time
                if (distance > slowing_distance)
                    physic.Force += direction / distance * max_force;
                else
                    physic.Force += -1 * physic.Velocity.Normalized() * max_force;
            }
            else
            {
                ai_node.AI.TargetPosition = null;
            }
        }

        protected void ProcessTurnToVelocity(double delta_time, AINode ai_node)
        {
            var physic = ai_node.Entity.Get<PhysicComponent>();

            if (physic.Velocity.LengthSquared <= DistanceEpsilonSquared)
                return;

            var max_torque = 1;
            var target = physic.Velocity;
            var actual = ai_node.Base.ForwardRelative;
            var direction = Vector3d.Cross(actual, target);
            var distance = 1 - Vector3d.Dot(target, actual) / target.Length / actual.Length;
            if (distance > RotationEpsilon)
            {
                var deceleration = physic.AngularVelocity.Length;
                var velocity_0 = physic.AngularVelocity.Length;
                var slowing_distance = System.Math.Pow(deceleration, 2) / 2 + velocity_0 * delta_time; // / max_torque * entity.Inertia; // slowing_distance = decelertation^2/2 + velocity_0 * delta_time
                if (distance > slowing_distance)
                    physic.Torque += direction.Normalized() * max_torque;
                else
                    physic.Torque += -1 * physic.AngularVelocity.Normalized() * max_torque;
            }
        }

        //protected void Steer(SteeringCommand steering_command, double delta_time)
        //{
        //    var physic = Subject.GetComponent<Physic>();

        //    if (steering_command.Force.LengthSquared > DistanceEpsilonSquared)
        //        physic.Force += steering_command.Force - physic.Velocity;

        //    if (steering_command.Torque.LengthSquared > DistanceEpsilonSquared)
        //        physic.Torque += steering_command.Torque - physic.AngularVelocity;
        //}

        //protected Vector3d Seek(Vector3d target, double delta_time)
        //{
        //    return target - Subject.Position;
        //}
    }
}
