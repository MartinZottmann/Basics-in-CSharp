using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Entities.Nodes;
using MartinZottmann.Game.Graphics;
using OpenTK;
using System;

namespace MartinZottmann.Game.Entities
{
    public class Creator
    {
        public static Random Random = new Random();

        public EntityManager EntityManager;

        public ResourceManager ResourceManager;

        public Creator(EntityManager entity_manager, ResourceManager resource_manager)
        {
            EntityManager = entity_manager;
            ResourceManager = resource_manager;
        }

        public Entity Create(string filepath, Vector3d? position = null, Matrix4? transformation = null, string program = "standard", string texture = "Resources/Textures/debug-256.png")
        {
            var g = new Graphic();
            g.Model = ResourceManager.Entities.Load(filepath, transformation);
            g.Model.Program = ResourceManager.Programs[program];
            g.Model.Texture = ResourceManager.Textures[texture];

            var b = new Base();
            if (position != null)
                b.Position = position.Value;

            var e = new Entity()
                .Add(b)
                .Add(new Physic() { BoundingBox = g.Model.Mesh().BoundingBox, BoundingSphere = g.Model.Mesh().BoundingSphere })
                .Add(g);
            EntityManager.Add(e);

            return e;
        }

        public Entity CreateAsteroid()
        {
            var scale = Random.NextDouble() * 5 + 1;

            var e = Create("Resources/Models/sphere.obj");
            var b = e.Get<Base>();
            b.Position = new Vector3d((Random.NextDouble() - 0.5) * 100.0, (Random.NextDouble() - 0.5) * 100.0, (Random.NextDouble() - 0.5) * 100.0);
            b.Scale = new Vector3d(scale);
            var p = e.Get<Physic>();
            p.Mass *= scale;
            var I = 2 * p.Mass * System.Math.Pow(scale, 2) / 5;
            p.Inertia = new Matrix4d(
                I, 0, 0, 0,
                0, I, 0, 0,
                0, 0, I, 0,
                0, 0, 0, 1
            );
            p.Velocity = new Vector3d((Random.NextDouble() - 0.5) * 1.0, (Random.NextDouble() - 0.5) * 1.0, (Random.NextDouble() - 0.5) * 1.0);
            p.AngularVelocity = new Vector3d((Random.NextDouble() - 0.5) * 1.0, (Random.NextDouble() - 0.5) * 1.0, (Random.NextDouble() - 0.5) * 1.0);

            return e;
        }

        public Entity CreateShip(Vector3d position)
        {
            Create("Resources/Models/cube.obj", position + new Vector3d(0, -1, -1), Matrix4.CreateScale(0.5f, 0.5f, 0.5f));
            Create("Resources/Models/cube.obj", position + new Vector3d(0, -1, 0), Matrix4.CreateScale(0.5f, 0.5f, 0.5f));
            Create("Resources/Models/cube.obj", position + new Vector3d(0, -1, 1), Matrix4.CreateScale(0.5f, 0.5f, 0.5f));
            Create("Resources/Models/table.obj", position + new Vector3d(0, 0, -1), Matrix4.CreateScale(0.5f, 0.5f, 0.5f));

            var e = new Entity("Ship", true)
                .Add(new Base() { Position = position })
                .Add(new Physic())
                .Add(new Target());
            EntityManager.Add(e);

            return e;
        }

        public Entity CreateCamera()
        {
            var e = new Entity("Camera")
                .Add(new Base())
                .Add(new Input());
            EntityManager.Add(e);

            return e;
        }

        public Entity CreateStarfield()
        {
            var g = new Graphic();
            g.Model = new Starfield();
            g.Model.Program = ResourceManager.Programs["normal"];

            var e = new Entity("Starfield")
                .Add(new Base())
                .Add(g);
            EntityManager.Add(e);

            return e;
        }

        public Entity CreateGrid()
        {
            var g = new Graphic();
            g.Model = new Grid();
            g.Model.Program = ResourceManager.Programs["normal"];

            var e = new Entity("Grid")
                .Add(new Base())
                .Add(g);
            EntityManager.Add(e);

            return e;
        }

        public Entity CreateCursor()
        {
            var g = new Graphic();
            g.Model = new MartinZottmann.Game.Graphics.Cursor();
            g.Model.Program = ResourceManager.Programs["normal"];

            var e = new Entity("Cursor")
                .Add(new Base())
                .Add(new MartinZottmann.Game.Entities.Components.Cursor())
                .Add(g);
            EntityManager.Add(e);

            return e;
        }
    }
}
