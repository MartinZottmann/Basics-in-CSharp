using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Graphics.Shapes;
using MartinZottmann.Engine.Physics;
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

            Mass *= scale;
            var I = 2 * Mass * System.Math.Pow(scale, 2) / 5;
            Inertia = new Matrix4d(
                I, 0, 0, 0,
                0, I, 0, 0,
                0, 0, I, 0,
                0, 0, 0, 1
            );

            var shape = new Sphere();
            shape.Translate(Matrix4.CreateScale((float)scale));

            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Mesh(shape);
            graphic.Program = Resources.Programs["standard_cube"];
            var texture = new Texture("Resources/Textures/debug-256.png", false, OpenTK.Graphics.OpenGL.TextureTarget.TextureCubeMap);
            graphic.Texture = texture;

            BoundingBox = shape.BoundingBox;
            BoundingSphere = shape.BoundingSphere;

            AddImpulse(
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

        protected Explosion explosion;

        public override void OnCollision(Collision collision)
        {
            base.OnCollision(collision);

            if (explosion != null && explosion.Destroyed)
                explosion = null;
            if (explosion == null)
            {
                explosion = new Explosion(Resources);
                ((Entity)collision.Parent).AddChild(explosion);
            }
            explosion.Position = collision.HitPoint;
            explosion.Age = 0.0;
            explosion.MaxAge = Explosion.Random.NextDouble() / 2;
        }
    }
}
