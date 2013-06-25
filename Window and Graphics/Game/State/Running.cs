using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities;
using MartinZottmann.Game.Entities.Helper;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace MartinZottmann.Game.State
{
    class Running : GameState
    {
        protected List<Entities.Entity> entities = new List<Entities.Entity>();

        protected List<Entities.Physical> selection = new List<Entities.Physical>();

        protected Camera camera;

        protected ResourceManager resources;

        protected Cursor cursor;

        protected RenderContext render_context;

        public Running(GameWindow window)
            : base(window)
        {
            resources = new ResourceManager();

            var shaders = new Dictionary<string, List<Shader>>();
            foreach (var filename in Directory.GetFiles("res/Shaders/", "*.glsl"))
            {
                System.Console.WriteLine(filename);
                var chunks = filename.Split(new char[] { '/', '.' });
                var name = chunks[chunks.Length - 3];
                var type = chunks[chunks.Length - 2];
                Shader shader;

                switch (type)
                {
                    case "vs":
                        shader = resources.Shaders.Load(ShaderType.VertexShader, filename);
                        break;
                    case "fs":
                        shader = resources.Shaders.Load(ShaderType.FragmentShader, filename);
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
                System.Console.WriteLine(shader.Key);
                resources.Programs.Load(shader.Key, shader.Value.ToArray());
            }

            foreach (var filename in Directory.GetFiles("res/textures/", "*.png"))
                resources.Textures.Load(filename, true, TextureTarget.Texture2D);

            camera = new Camera(Window);
            //camera.MouseLook = true;
            camera.Position.X = 100;
            camera.Position.Y = 100;
            camera.Position.Z = 100;
            camera.Direction.X = -1;
            camera.Direction.Y = -1;
            camera.Direction.Z = -1;
            camera.Direction.NormalizeFast();

            Window.Keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(OnKeyUp);

            Add(new Entities.GUI.FPSCounter(resources));

            cursor = new Cursor(resources);
            Window.Mouse.ButtonUp += (s, e) =>
            {
                if (e.Button == MouseButton.Left)
                {
                    foreach (var entity in selection)
                        entity.Mark = new OpenTK.Graphics.Color4(255, 255, 0, 127);
                    selection.Clear();

                    Entities.Physical g_entity = null;
                    Entities.Physical s_entity = null;
                    var g_min = Double.MaxValue;
                    var g_max = Double.MinValue;
                    var g_s_min = Double.MaxValue;
                    var g_s_max = Double.MinValue;
                    foreach (Entities.Entity entity in entities)
                    {
                        if (entity is Physical)
                        {
                            double l_min;
                            double l_max;
                            if ((entity as Physical).BoundingBox.Intersect(ref cursor.Ray, entity.Position, out l_min, out l_max))
                                if (l_min < g_min)
                                {
                                    g_entity = (entity as Physical);
                                    g_min = l_min;
                                    g_max = l_max;
                                }

                            double l_s_min;
                            double l_s_max;
                            if ((entity as Physical).BoundingSphere.Intersect(ref cursor.Ray, entity.Position, out l_s_min, out l_s_max))
                                if (l_s_min < g_s_min)
                                {
                                    s_entity = (entity as Physical);
                                    g_s_min = l_s_min;
                                    g_s_max = l_s_max;
                                }
                        }
                    }
                    if (g_entity != null)
                    {
                        //selection.Add(g_entity);
                        g_entity.Mark = new OpenTK.Graphics.Color4(255, 255, 0, 255);
                    }
                    if (s_entity != null)
                    {
                        selection.Add(s_entity);
                        s_entity.Mark = new OpenTK.Graphics.Color4(255, 0, 255, 255);
                    }
                }

                if (e.Button == MouseButton.Right)
                    foreach (var entity in selection)
                        if (entity is INavigation)
                            (entity as INavigation).Target = cursor.Position;
            };
            Add(cursor);

            Add(new Grid(resources));

            Add(new Starfield(resources));

            for (int i = 1; i <= 10; i++)
                Add(new Asteroid(resources));

            Add(new Textured(resources));

            Add(new Ship(resources));
        }

        public override void Dispose()
        {
            cursor = null;

            entities.Clear();

            resources.Dispose();
        }

        protected void OnKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.F10)
                camera.MouseLook = !camera.MouseLook;
        }

        protected void Add(Entities.Entity entity)
        {
            entities.Add(entity);
        }

        public override void Update(double delta_time)
        {
            if (Window.Keyboard[Key.W])
                if (camera.MouseLook)
                    camera.Position += camera.Direction * delta_time * 100;
                else
                    camera.Position += camera.Forward * delta_time * 100;
            if (Window.Keyboard[Key.S])
                if (camera.MouseLook)
                    camera.Position -= camera.Direction * delta_time * 100;
                else
                    camera.Position -= camera.Forward * delta_time * 100;
            if (Window.Keyboard[Key.A])
                camera.Position -= camera.Right * delta_time * 100;
            if (Window.Keyboard[Key.D])
                camera.Position += camera.Right * delta_time * 100;
            if (Window.Keyboard[Key.Space])
                camera.Position += camera.Up * delta_time * 100;
            if (Window.Keyboard[Key.ShiftLeft])
                camera.Position -= camera.Up * delta_time * 100;
            if (Window.Keyboard[Key.F])
                camera.RotateDirectionAroundUp(delta_time);
            if (Window.Keyboard[Key.H])
                camera.RotateDirectionAroundUp(-delta_time);
            if (Window.Keyboard[Key.T])
                camera.RotateDirectionAroundRight(delta_time);
            if (Window.Keyboard[Key.G])
                camera.RotateDirectionAroundRight(-delta_time);
            if (Window.Keyboard[Key.KeypadPlus])
                camera.Fov += MathHelper.PiOver6 * delta_time;
            if (Window.Keyboard[Key.KeypadSubtract])
                camera.Fov -= MathHelper.PiOver6 * delta_time;
            if (camera.Fov <= 0)
                camera.Fov = delta_time;
            if (camera.Fov > System.Math.PI)
                camera.Fov = System.Math.PI;
            if (Window.Keyboard[Key.J])
                foreach (var entity in selection)
                {
                    entity.AddForceRelative(new Vector3d(0, 0, -1), new Vector3d(10, 0, 0) * delta_time);
                    entity.AddForceRelative(new Vector3d(0, 0, 1), new Vector3d(-10, 0, 0) * delta_time);
                }
            if (Window.Keyboard[Key.L])
                foreach (var entity in selection)
                {
                    entity.AddForceRelative(new Vector3d(0, 0, -1), new Vector3d(-10, 0, 0) * delta_time);
                    entity.AddForceRelative(new Vector3d(0, 0, 1), new Vector3d(10, 0, 0) * delta_time);
                }
            if (Window.Keyboard[Key.I])
                foreach (var entity in selection)
                {
                    entity.AddForceRelative(new Vector3d(0, 0, -1), new Vector3d(0, 10, 0) * delta_time);
                    entity.AddForceRelative(new Vector3d(0, 0, 1), new Vector3d(0, -10, 0) * delta_time);
                }
            if (Window.Keyboard[Key.K])
                foreach (var entity in selection)
                {
                    entity.AddForceRelative(new Vector3d(0, 0, -1), new Vector3d(0, -10, 0) * delta_time);
                    entity.AddForceRelative(new Vector3d(0, 0, 1), new Vector3d(0, 10, 0) * delta_time);
                }
            if (Window.Keyboard[Key.U])
                foreach (var entity in selection)
                {
                    entity.AddForceRelative(new Vector3d(-1, 0, 0), new Vector3d(0, -10, 0) * delta_time);
                    entity.AddForceRelative(new Vector3d(1, 0, 0), new Vector3d(0, 10, 0) * delta_time);
                }
            if (Window.Keyboard[Key.O])
                foreach (var entity in selection)
                {
                    entity.AddForceRelative(new Vector3d(-1, 0, 0), new Vector3d(0, 10, 0) * delta_time);
                    entity.AddForceRelative(new Vector3d(1, 0, 0), new Vector3d(0, -10, 0) * delta_time);
                }
            if (Window.Keyboard[Key.M])
                foreach (var entity in selection)
                    entity.AngularVelocity = Vector3d.Zero;


            camera.Update(delta_time);

            render_context = new RenderContext()
            {
                Window = Window,
                Camera = camera,
                Projection = camera.ProjectionMatrix(),
                View = camera.ViewMatrix()
            };

            foreach (Entities.Entity entity in entities)
            {
                entity.Update(delta_time, render_context);

                entity.Reposition(100, 100, 100);
            }
        }

        public override void Render(double delta_time)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Window.Width, Window.Height);

            foreach (Entities.Entity entity in entities)
            {
                //render_context.Model = entity.Model;
                entity.Render(delta_time, render_context);
            }

            Window.SwapBuffers();
        }
    }
}
