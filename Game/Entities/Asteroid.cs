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

            graphic.Program.UniformLocations["in_Texture"].Set(0);
            graphic.Program.UniformLocations["in_LightPosition"].Set(new Vector3(10, 10, 10));

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
            graphic.Program.UniformLocations["in_Model"].Set(render_context.Model);
            graphic.Program.UniformLocations["in_View"].Set(render_context.View);
            graphic.Program.UniformLocations["in_ModelView"].Set(render_context.ViewModel);
            graphic.Program.UniformLocations["in_ModelViewProjection"].Set(render_context.ProjectionViewModel);
            graphic.Program.UniformLocations["in_NormalView"].Set(render_context.Normal);

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
