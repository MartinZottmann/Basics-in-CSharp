using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Graphics;
using OpenTK;

namespace MartinZottmann.Game.Entities
{
    public class Asteroid : GameObject
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

            var physic = AddComponent(new Physic(this));
            physic.Mass *= scale;
            var I = 2 * physic.Mass * System.Math.Pow(scale, 2) / 5;
            physic.Inertia = new Matrix4d(
                I, 0, 0, 0,
                0, I, 0, 0,
                0, 0, I, 0,
                0, 0, 0, 1
            );

            Scale = new Vector3d(scale);

            var graphic = AddComponent(new Graphic(this));
            graphic.Model = Resources.Entities.Load("Resources/Models/sphere.obj", Matrix4.CreateScale(0.5f, 0.5f, 0.5f));
            var shape = graphic.Model.Mesh();

            graphic.Model.Program = Resources.Programs["standard"];
            graphic.Model.Texture = Resources.Textures["Resources/Textures/debug-256.png"];

            physic.BoundingBox = shape.BoundingBox;
            physic.BoundingSphere = shape.BoundingSphere;

            physic.AddImpulse(
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
