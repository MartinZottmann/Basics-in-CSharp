using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics;
using MartinZottmann.Game.Entities;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Entities.GUI;
using MartinZottmann.Game.Entities.Systems;
using MartinZottmann.Game.Input;
using MartinZottmann.Game.IO;
using MartinZottmann.Game.Resources;
using OpenTK;
using System;
using System.Diagnostics;

namespace MartinZottmann.Game.State
{
    public class Running : GameState
    {
        protected ResourceLoader resource_loader;

        protected FileSystem file_system = new FileSystem("save.xml");

        protected EntityManager entity_manager;

        public Running(Window window)
            : base(window)
        {
            resource_loader = new ResourceLoader();
        }

        public override void Start()
        {
            resource_loader.LoadPrograms("Resources/Shaders/", "*.glsl");
            resource_loader.LoadTextures("Resources/Textures/", "*.png");

            var world_camera = new Camera(Window);
            world_camera.Position = new Vector3d(10);
            world_camera.LookAt = new Vector3d(0);
            var screen_camera = new Camera(Window);

            var input_manager = new InputManager(Window);

            entity_manager = new EntityManager();
            entity_manager.AddSystem(new GameStateSystem());
            entity_manager.AddSystem(new InputSystem(input_manager, world_camera));
            entity_manager.AddSystem(new CameraSystem(world_camera));
            entity_manager.AddSystem(new SelectionSystem(input_manager, world_camera));
            entity_manager.AddSystem(new AISystem());
            entity_manager.AddSystem(new PhysicSystem());
            entity_manager.AddSystem(new CollisionSystem());
            entity_manager.AddSystem(new ChunkSystem());
            entity_manager.AddSystem(new GraphicSystem(world_camera, resource_loader.Manager));
            entity_manager.AddSystem(new ParticleSystem(world_camera, resource_loader.Manager));
            entity_manager.AddSystem(new GUISystem(input_manager, screen_camera, resource_loader.Manager));

            Entity[] entities = null;
            try
            {
                Debug.WriteLine(String.Format("Loading {0}", file_system.FilePath), GetType().FullName);
                //entities = file_system.Load<Entity[]>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            if (null == entities)
            {
                var creator = new Creator(entity_manager, resource_loader.Manager);
                creator.CreateGameState();
                creator.CreateCamera(world_camera.Position, world_camera.Orientation);
                creator.CreateStarfield();
                creator.CreateGrid();
                for (var i = 0; i < 10; i++)
                    creator.CreateAsteroid();
                //var a0 = creator.Create("Resources/Models/sphere.obj", new Vector3d(-5, 0, 0));
                //a0.Add(new CollisionComponent() { Group = CollisionGroups.Space });
                //a0.Get<PhysicComponent>().Velocity = new Vector3d(1, 0, 0);
                //var a1 = creator.Create("Resources/Models/sphere.obj", new Vector3d(5, 0, 0));
                //a1.Add(new CollisionComponent() { Group = CollisionGroups.Space });
                //a1.Get<PhysicComponent>().Velocity = new Vector3d(-1, 0, 0);
                //creator.CreateShip(new Vector3d(-5, 0, 0)).Get<PhysicComponent>().AngularVelocity = Vector3d.UnitX;
                //creator.CreateShip(new Vector3d(0, 0, 0))
                //    .Add(new ParticleEmitterComponent())
                //    .Get<PhysicComponent>().AngularVelocity = Vector3d.UnitY;
                //creator.CreateShip(new Vector3d(5, 0, 0)).Get<PhysicComponent>().AngularVelocity = Vector3d.UnitZ;
                for (var i = 0; i < 10; i++)
                    creator.CreateShip(new Vector3d(5 * i, 0, 0));
                //creator.Create("Resources/Models/cube.obj", new Vector3d(0, -20, 0), new Vector3d(10));
                //creator.Create("Resources/Models/cube.obj", new Vector3d(-20, 0, 0), new Vector3d(10));
            }
            else
            {
                foreach (var entity in entities)
                    entity_manager.AddEntity(entity);

                var camera_base = entity_manager.GetEntity("Camera").Get<BaseComponent>();
                world_camera.Position = camera_base.Position;
                world_camera.Orientation = camera_base.Orientation;
            }

            entity_manager.GetSystem<GUISystem>().Add(new MartinZottmann.Game.Entities.GUI.Debugger(entity_manager));
            entity_manager.GetSystem<GUISystem>().Add(new FPSCounter());
            entity_manager.GetSystem<GUISystem>().Add(new Crosshair());

            Debug.WriteLine(String.Format("Saving {0}", file_system.FilePath), GetType().FullName);
            file_system.Save(entity_manager.Entities);
        }

        public override void Update(double delta_time)
        {
            entity_manager.Update(delta_time);
        }

        public override void Render(double delta_time)
        {
            entity_manager.Render(delta_time);

            Window.SwapBuffers();
        }

        public override void Stop()
        {
            Debug.WriteLine(String.Format("Saving {0}", file_system.FilePath), GetType().FullName);
            file_system.Save(entity_manager.Entities);

            entity_manager.Clear();
            resource_loader.Manager.Clear();
        }
    }
}
