using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Nodes;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace MartinZottmann.Game.Entities.Systems
{
    public class ParticleList
    {
        public Model Model;

        public VertexP3C4[] Verticies;

        public Particle[] Particles;

        public BufferObject<VertexP3C4> BufferObject;
    }

    public struct Particle
    {
        public double Age;

        public Vector3 Direction;

        public double MaxAge;
    }

    public class ParticleSystem : ISystem
    {
        public static Random Random = new Random();

        public Camera Camera;

        public ResourceManager ResourceManager;

        protected NodeList<ParticleEmitterNode> particle_emitter_nodes;

        protected Dictionary<string, ParticleList> particles = new Dictionary<string, ParticleList>();

        public ParticleSystem(Camera camera, ResourceManager resource_manager)
        {
            Camera = camera;
            ResourceManager = resource_manager;
        }

        public void Bind(EntityManager entitiy_manager)
        {
            particle_emitter_nodes = entitiy_manager.Get<ParticleEmitterNode>();
            particle_emitter_nodes.NodeAdded += Init;
            particle_emitter_nodes.NodeRemoved += UnInit;
        }

        public void Update(double delta_time)
        {
            foreach (var particle_emitter_node in particle_emitter_nodes)
            {
                var p = particles[particle_emitter_node.Entity.Name];
                for (var i = 0; i < particle_emitter_node.ParticleEmitter.NumParticles; i++)
                {
                    p.Particles[i].Age += delta_time;
                    p.Verticies[i].Position += p.Particles[i].Direction;

                    if (p.Particles[i].Age >= p.Particles[i].MaxAge)
                    {
                        p.Verticies[i] = GetParticleVertex();
                        p.Particles[i] = GetParticle();
                    }
                }

                p.BufferObject.Write(p.Verticies);
            }
        }

        public void Render(double delta_time)
        {
            foreach (var particle_emitter_node in particle_emitter_nodes)
            {
                var p = particles[particle_emitter_node.Entity.Name];
                p.Model.Program.UniformLocations["in_ModelViewProjection"].Set(Matrix4d.CreateTranslation(particle_emitter_node.Base.Position) * Camera.ViewMatrix * Camera.ProjectionMatrix);
                p.Model.Program.UniformLocations["in_CameraPosition"].Set(Camera.Position);
                p.Model.Program.UniformLocations["in_CameraUp"].Set(Camera.Up);
                p.Model.Program.UniformLocations["in_ParticleSize"].Set(0.25f);
                p.Model.Program.UniformLocations["in_Texture"].Set(0);
                GL.DepthMask(false);
                p.Model.Draw();
                GL.DepthMask(true);
            }
        }

        protected void Init(object sender, NodeEventArgs<ParticleEmitterNode> e)
        {
            var n = e.Node.ParticleEmitter.NumParticles;
            var p = new ParticleList();

            p.Verticies = new VertexP3C4[n];
            p.Particles = new Particle[n];
            for (var i = 0; i < n; i++)
            {
                p.Verticies[i] = GetParticleVertex();
                p.Particles[i] = GetParticle();
            }

            p.Model = new Model();
            p.Model.Mesh(new Mesh<VertexP3C4, uint>(p.Verticies));
            p.Model.Mode = BeginMode.Points;
            p.Model.Program = ResourceManager.Programs["particle"];
            p.Model.Texture = ResourceManager.Textures["Resources/Textures/particle-dot.png"];

            p.BufferObject = (BufferObject<VertexP3C4>)p.Model.VertexArrayObject.BufferObjects[0];

            particles.Add(e.Node.Entity.Name, p);
        }

        protected void UnInit(object sender, NodeEventArgs<ParticleEmitterNode> e)
        {
            particles.Remove(e.Node.Entity.Name);
        }

        protected VertexP3C4 GetParticleVertex()
        {
            var i = (float)(Random.NextDouble() * 0.5 + 0.5);
            var j = (float)(Random.NextDouble() * 0.5 + 0.5);
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
            particle.Direction *= (float)(Random.NextDouble() * 0.5);
            particle.MaxAge = Random.NextDouble() * 2.0;
            return particle;
        }
    }
}
