using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Game.Entities
{
    public struct Particle
    {
        public double Age;

        public Vector3 Direction;

        public double MaxAge;
    }

    public class Explosion : Drawable
    {
        const int num_particles = 100;

        protected VertexP3C4[] particle_verticies;

        protected Particle[] particles;

        protected BufferObject<VertexP3C4> buffer_object;

        public double Age = 0.0;

        public double MaxAge = System.Double.MaxValue;

        public Explosion(ResourceManager resources)
            : base(resources)
        {
            Position = new Vector3d(
                (Random.NextDouble() - 0.5) * 100.0,
                (Random.NextDouble() - 0.5) * 100.0,
                (Random.NextDouble() - 0.5) * 100.0
            );

            particle_verticies = new VertexP3C4[num_particles];
            particles = new Particle[num_particles];
            for (var i = 0; i < num_particles; i++)
            {
                particle_verticies[i] = GetParticleVertex();
                particles[i] = GetParticle();
            }

            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Mesh(new Mesh<VertexP3C4, uint>(particle_verticies));
            graphic.Mode = BeginMode.Points;
            graphic.Program = Resources.Programs["normal"];

            buffer_object = graphic.VertexArrayObject.BufferObjects[0] as BufferObject<VertexP3C4>;
        }

        public override void Update(double delta_time, RenderContext render_context)
        {
            Age += delta_time;

            if (Age >= MaxAge)
                Destroyed = true;

            if (Destroyed)
                return;

            for (var i = 0; i < num_particles; i++)
            {
                particle_verticies[i].Position += particles[i].Direction;
                particles[i].Age += delta_time;
                if (particles[i].Age > particles[i].MaxAge)
                {
                    particle_verticies[i] = GetParticleVertex();
                    particles[i] = GetParticle();
                }
            }
            buffer_object.Write(particle_verticies);

            base.Update(delta_time, render_context);
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            if (Destroyed)
                return;

            render_context.Model = Model;
            graphic.Program.UniformLocations["PVM"].Set(render_context.ProjectionViewModel);

            GL.PointSize(2);
            base.Render(delta_time, render_context);
        }

        protected VertexP3C4 GetParticleVertex()
        {
            var i = (float)(Random.NextDouble() / 2 + 0.5);
            var j = (float)(Random.NextDouble() / 2 + 0.5);
            return new VertexP3C4(
                0,
                0,
                0,
                System.Math.Max(i, j),
                System.Math.Min(i, j),
                0,
                (float)Random.NextDouble()
            );
        }

        protected Particle GetParticle()
        {
            var particle = new Particle();
            particle.Age = 0.0;
            particle.Direction.X = (float)(Random.NextDouble() - 0.5);
            particle.Direction.Y = (float)(Random.NextDouble() - 0.5);
            particle.Direction.Z = (float)(Random.NextDouble() - 0.5);
            particle.Direction.Normalize();
            particle.Direction *= (float)(Random.NextDouble() / 2);
            particle.MaxAge = Random.NextDouble() * 2;
            return particle;
        }
    }
}
