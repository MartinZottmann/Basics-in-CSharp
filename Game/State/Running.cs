using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Entities.GUI;
using MartinZottmann.Game.Entities.Systems;
using MartinZottmann.Game.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MartinZottmann.Game.State
{
    public class Running : GameState
    {
        protected ResourceManager resource_manager = new ResourceManager();

        protected FileSystem file_system = new FileSystem("save.xml");

        protected MartinZottmann.Game.Graphics.RenderContext screen_render_context = new MartinZottmann.Game.Graphics.RenderContext();

        protected EntityManager entity_manager;

        public Running(Window window)
            : base(window)
        {
            var shaders = new Dictionary<string, List<Shader>>();
            foreach (var filename in Directory.GetFiles("Resources/Shaders/", "*.glsl"))
            {
                Console.WriteLine(filename);
                var chunks = filename.Split(new char[] { '/', '.' });
                var name = chunks[chunks.Length - 3];
                var type = chunks[chunks.Length - 2];
                Shader shader;

                switch (type)
                {
                    case "vs":
                        shader = resource_manager.Shaders.Load(ShaderType.VertexShader, filename);
                        break;
                    case "gs":
                        shader = resource_manager.Shaders.Load(ShaderType.GeometryShader, filename);
                        break;
                    case "fs":
                        shader = resource_manager.Shaders.Load(ShaderType.FragmentShader, filename);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                if (!shaders.ContainsKey(name))
                    shaders.Add(name, new List<Shader>());
                shaders[name].Add(shader);
            }
            foreach (var shader in shaders)
            {
                Console.WriteLine(shader.Key);
                resource_manager.Programs.Load(shader.Key, shader.Value.ToArray());
            }

            foreach (var filename in Directory.GetFiles("Resources/Textures/", "*.png"))
                resource_manager.Textures.Load(filename, true, TextureTarget.Texture2D);

            Window.Keyboard.KeyUp += OnKeyUp;

            var world_camera = new Camera(Window);
            world_camera.Position = new Vector3d(10);
            world_camera.LookAt = new Vector3d(0);
            var screen_camera = new Camera(Window);

            entity_manager = new EntityManager()
                .Add(new InputSystem(Window, world_camera))
                .Add(new CursorSystem(Window, world_camera))
                .Add(new GameStateSystem())
                .Add(new AISystem())
                .Add(new PhysicSystem())
                .Add(new CollisionSystem())
                .Add(new ChunkSystem())
                .Add(new GraphicSystem(world_camera, resource_manager))
                .Add(new ParticleSystem(world_camera, resource_manager))
                .Add(new GUISystem(Window, screen_camera, resource_manager));

            MartinZottmann.Engine.Entities.Entity[] entities = null;
            try
            {
                Debug.WriteLine(String.Format("Loading {0}", file_system.FilePath));
                //entities = file_system.Load<MartinZottmann.Engine.Entities.Entity[]>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            if (null == entities)
            {
                var creator = new Creator(entity_manager, resource_manager);
                creator.CreateGameState();
                creator.CreateCursor();
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
                creator.CreateShip(new Vector3d(-5, 0, 0)).Get<PhysicComponent>().AngularVelocity = Vector3d.UnitX;
                creator.CreateShip(new Vector3d(0, 0, 0))
                    .Add(new ParticleEmitterComponent())
                    .Get<PhysicComponent>().AngularVelocity = Vector3d.UnitY;
                creator.CreateShip(new Vector3d(5, 0, 0)).Get<PhysicComponent>().AngularVelocity = Vector3d.UnitZ;
                creator.Create("Resources/Models/cube.obj", new Vector3d(0, -20, 0), new Vector3d(10));
                creator.Create("Resources/Models/cube.obj", new Vector3d(-20, 0, 0), new Vector3d(10));
            }
            else
            {
                foreach (var entity in entities)
                    entity_manager.Add(entity);

                var camera_base = entity_manager.Get("Camera").Get<BaseComponent>();
                world_camera.Position = camera_base.Position;
                world_camera.Orientation = camera_base.Orientation;
            }

            entity_manager.GetSystem<GUISystem>().Add(new MartinZottmann.Game.Entities.GUI.Debugger(entity_manager));
            entity_manager.GetSystem<GUISystem>().Add(new FPSCounter());

            Debug.WriteLine(String.Format("Saving {0}", file_system.FilePath));
            file_system.Save(entity_manager.Entities);
        }

        public override void Dispose()
        {
            Debug.WriteLine(String.Format("Saving {0}", file_system.FilePath));
            file_system.Save(entity_manager.Entities);
            entity_manager.Dispose();

            resource_manager.Dispose();
        }

        protected void OnKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                entity_manager.Get("GameState").Get<GameStateComponent>().Debug = !entity_manager.Get("GameState").Get<GameStateComponent>().Debug;
            }
            if (e.Key == Key.F10)
            {
                var camera = entity_manager.GetSystem<InputSystem>().Camera;
                camera.MouseLook = !camera.MouseLook;
            }
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
    }
}
