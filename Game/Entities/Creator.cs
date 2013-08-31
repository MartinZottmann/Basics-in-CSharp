using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Components;
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

        public Entity Create(string filepath, Vector3d? position = null, Vector3d? scale = null, string program = "standard", string texture = "Resources/Textures/debug-256.png")
        {
            var g = new GraphicComponent();
            g.ModelName = filepath;
            g.ProgramName = program;
            g.TextureName = texture;
            g.Model = ResourceManager.Models.Load(filepath);
            g.Model.Program = ResourceManager.Programs[program];
            g.Model.Texture = ResourceManager.Textures[texture];

            var b = new BaseComponent();
            if (scale != null)
                b.Scale = scale.Value;
            if (position != null)
                b.Position = position.Value;

            var bb = g.Model.Mesh().BoundingBox;
            if (scale != null)
            {
                bb.Max = Vector3d.Multiply(bb.Max, scale.Value);
                bb.Min = Vector3d.Multiply(bb.Min, scale.Value);
            }

            var bs = g.Model.Mesh().BoundingSphere;
            if (scale != null)
                bs.Radius *= scale.Value.Length;

            var e = new Entity(filepath, true)
                .Add(b)
                .Add(new PhysicComponent() { BoundingBox = bb, BoundingSphere = bs })
                .Add(g);
            EntityManager.Add(e);

            return e;
        }

        public Entity CreateAsteroid()
        {
            var scale = Random.NextDouble() * 5 + 1;

            var e = Create("Resources/Models/sphere.obj", new Vector3d((Random.NextDouble() - 0.5) * 100.0, (Random.NextDouble() - 0.5) * 100.0, (Random.NextDouble() - 0.5) * 100.0), new Vector3d(scale));
            var p = e.Get<PhysicComponent>();
            p.Mass *= scale;
            var I = 2 * p.Mass * Math.Pow(scale, 2) / 5;
            p.Inertia = new Matrix4d(
                I, 0, 0, 0,
                0, I, 0, 0,
                0, 0, I, 0,
                0, 0, 0, 1
            );
            p.Velocity = new Vector3d((Random.NextDouble() - 0.5) * 10.0, (Random.NextDouble() - 0.5) * 10.0, (Random.NextDouble() - 0.5) * 10.0);
            p.AngularVelocity = new Vector3d((Random.NextDouble() - 0.5) * 1.0, (Random.NextDouble() - 0.5) * 1.0, (Random.NextDouble() - 0.5) * 1.0);
            e.Add(new CollisionComponent() { Group = CollisionGroups.Space });

            return e;
        }

        public Entity CreateShip(Vector3d position)
        {
            var c = new Entity[] {
                Create("Resources/Models/cube.obj", new Vector3d(0, -1, -1), new Vector3d(0.5f, 0.5f, 0.5f)),
                Create("Resources/Models/cube.obj", new Vector3d(0, -1, 0), new Vector3d(0.5f, 0.5f, 0.5f)),
                Create("Resources/Models/cube.obj", new Vector3d(0, -1, 1), new Vector3d(0.5f, 0.5f, 0.5f)),
                Create("Resources/Models/table.obj", new Vector3d(0, 0, -1), new Vector3d(0.5f, 0.5f, 0.5f))
            };

            var p = new PhysicComponent();
            foreach (var i in c)
                p.BoundingSphere.Radius = Math.Max(
                    p.BoundingSphere.Radius,
                    i.Get<BaseComponent>().Position.Length + i.Get<PhysicComponent>().BoundingSphere.Radius
                );
            p.BoundingBox.Max = new Vector3d(p.BoundingSphere.Radius);
            p.BoundingBox.Min = -p.BoundingBox.Max;

            var e = new Entity("Ship", true)
                .Add(new BaseComponent() { Position = position })
                .Add(p)
                .Add(new TargetComponent());

            foreach (var i in c)
                i.Get<BaseComponent>().ParentName = e.Name;

            EntityManager.Add(e);

            return e;
        }

        public Entity CreateCamera()
        {
            var e = new Entity("Camera")
                .Add(new BaseComponent())
                .Add(new InputComponent());
            EntityManager.Add(e);

            return e;
        }

        public Entity CreateStarfield()
        {
            var g = new GraphicComponent();
            g.ModelName = typeof(Starfield).FullName;
            g.ProgramName = "normal";
            //g.Model = new Starfield();
            //g.Model.Program = ResourceManager.Programs["normal"];

            var e = new Entity("Starfield")
                .Add(new BaseComponent())
                .Add(g);
            EntityManager.Add(e);

            return e;
        }

        public Entity CreateGrid()
        {
            var g = new GraphicComponent();
            g.ModelName = typeof(Grid).FullName;
            g.ProgramName = "normal";
            //g.Model = new Grid();
            //g.Model.Program = ResourceManager.Programs["normal"];

            var e = new Entity("Grid")
                .Add(new BaseComponent())
                .Add(g);
            EntityManager.Add(e);

            return e;
        }

        public Entity CreateCursor()
        {
            var g = new GraphicComponent();
            g.ModelName = typeof(Cursor).FullName;
            g.ProgramName = "normal";
            //g.Model = new MartinZottmann.Game.Graphics.Cursor();
            //g.Model.Program = ResourceManager.Programs["normal"];

            var e = new Entity("Cursor")
                .Add(new BaseComponent())
                .Add(new CursorComponent())
                .Add(g);
            EntityManager.Add(e);

            return e;
        }

        public Entity CreateGameState()
        {
            var e = new Entity("GameState")
                .Add(new GameStateComponent());
            EntityManager.Add(e);

            return e;
        }
    }
}
