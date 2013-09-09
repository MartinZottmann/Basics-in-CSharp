using MartinZottmann.Engine.Physics;
using MartinZottmann.Game.Entities.Components;
using OpenTK;
using System;

namespace MartinZottmann.Game.Entities.Nodes
{
    public class CollisionNode : PhysicNode
    {
        public CollisionComponent Collision;

        public void OnCollision(Collision collision)
        {
            if (this != collision.Object0)
                throw new Exception();

            var o0 = (CollisionNode)collision.Object0;
            var o1 = (CollisionNode)collision.Object1;
            var i0 = o0.InverseInertiaWorld;
            var i1 = o1.InverseInertiaWorld;
            var r0 = collision.HitPoint - o0.Base.Position;
            var r1 = collision.HitPoint - o1.Base.Position;
            var p0 = o0.Physic;
            var p1 = o1.Physic;
            var v0 = p0.Velocity + Vector3d.Cross(p0.AngularVelocity, r0);
            var v1 = p1.Velocity + Vector3d.Cross(p1.AngularVelocity, r1);
            var dv = v0 - v1;
            var n = collision.Normal.Normalized();

            //if (-Vector3d.Dot(dv, n) < -0.01)
            //    return;

            #region NORMAL Impulse
            {
                var kn = p0.InverseMass
                    + p1.InverseMass
                    + Vector3d.Dot(
                        n,
                        Vector3d.Cross(Vector3d.Transform(Vector3d.Cross(r0, n), i0), r0)
                        + Vector3d.Cross(Vector3d.Transform(Vector3d.Cross(r1, n), i1), r1)
                    );
                var dvn = Vector3d.Dot(dv, n);

                //var allowed_penetration_depth = 0.1;
                //var bias_factor = 0.1; // [0.1, 0.3]
                //var inv_dt = 1.0 / delta_time;
                //var bias = bias_factor * inv_dt * Math.Max(0.0, collision.PenetrationDepth - allowed_penetration_depth);
                //var P = Math.Max((-dvn + bias) / kn, 0);

                var e = 1; // coefficient of restitution [0, 1]
                var P = Math.Max(-(1 + e) * dvn / kn, 0);

                //var P = Math.Max(-dvn / kn, 0);
                var Pn = P * n;

                o0.AddImpulse(r0, Pn);
                o1.AddImpulse(r1, -Pn);
            }
            #endregion

            #region TANGENT Impulse
            {
                var tangent = (dv - Vector3d.Dot(dv, n) * n);
                var tl = tangent.Length;
                if (tl == 0)
                    return;
                tangent /= tl;
                var kt = p0.InverseMass
                    + p1.InverseMass
                    + Vector3d.Dot(
                        tangent,
                        Vector3d.Cross(Vector3d.Transform(Vector3d.Cross(r0, tangent), i0), r0)
                        + Vector3d.Cross(Vector3d.Transform(Vector3d.Cross(r1, tangent), i1), r1)
                    );
                var dvt = Vector3d.Dot(dv, tangent);
                var P = -dvt / kt;
                var Pt = P * tangent;

                o0.AddImpulse(r0, Pt);
                o1.AddImpulse(r1, -Pt);
            }
            #endregion
        }
    }
}
