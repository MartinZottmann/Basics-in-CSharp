using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Graphics;
using OpenTK;

namespace MartinZottmann.Game.Entities
{
    class Asteroid : Physical
    {
        public Asteroid(ResourceManager resources)
            : base(resources)
        {
            Position = new Vector3d(
                (Random.NextDouble() - 0.5) * 100.0,
                (Random.NextDouble() - 0.5) * 100.0,
                (Random.NextDouble() - 0.5) * 100.0
            );

            var scale = Random.NextDouble() * 5 + 1;

            Physic.Mass *= scale;
            var I = 2 * Physic.Mass * System.Math.Pow(scale, 2) / 5;
            Physic.Inertia = new Matrix4d(
                I, 0, 0, 0,
                0, I, 0, 0,
                0, 0, I, 0,
                0, 0, 0, 1
            );

            Scale = new Vector3d(scale);

            Graphic.Model = Resources.Entities.Load("Resources/Models/sphere.obj", Matrix4.CreateScale(0.5f, 0.5f, 0.5f));
            var shape = Graphic.Model.Mesh();

            Graphic.Model.Program = Resources.Programs["standard"];
            Graphic.Model.Texture = Resources.Textures["Resources/Textures/debug-256.png"];

            Physic.BoundingBox = shape.BoundingBox;
            Physic.BoundingSphere = shape.BoundingSphere;

            Physic.AddImpulse(
                Vector3d.Zero,
                new Vector3d(
                    (Random.NextDouble() - 0.5) * 100.0,
                    (Random.NextDouble() - 0.5) * 100.0,
                    (Random.NextDouble() - 0.5) * 100.0
                )
            );
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            render_context.Model = Model;
            base.Render(delta_time, render_context);
        }

        // @todo
        //protected Explosion explosion;

        //public override void OnCollision(Collision collision)
        //{
        //    base.OnCollision(collision);

        //    if (explosion != null && explosion.Destroyed)
        //        explosion = null;
        //    if (explosion == null)
        //    {
        //        explosion = new Explosion(Resources);
        //        ((Entity)collision.Parent).AddChild(explosion);
        //    }
        //    explosion.Position = collision.HitPoint;
        //    explosion.Age = 0.0;
        //    explosion.MaxAge = Explosion.Random.NextDouble() / 2;
        //}
    }
}
