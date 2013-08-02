using MartinZottmann.Engine;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities;
using MartinZottmann.Game.Entities.GUI;
using MartinZottmann.Game.Entities.Helper;
using MartinZottmann.Game.Entities.Ships;
using MartinZottmann.Game.Graphics;
using MartinZottmann.Game.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.IO;
using Camera = MartinZottmann.Engine.Graphics.Camera;

namespace MartinZottmann.Game.State
{
    class Running : GameState
    {
        protected World world;

        protected List<Entities.Physical> selection = new List<Entities.Physical>();

        protected Camera camera;

        protected ResourceManager resources;

        protected Cursor cursor;

        protected RenderContext world_render_context = new RenderContext();

        protected FileSystem file_system;

        protected Screen screen;

        protected RenderContext screen_render_context = new RenderContext();

        protected string savegame_filepath;

        public Running(Window window)
            : base(window)
        {
            resources = new ResourceManager();

            var shaders = new Dictionary<string, List<Shader>>();
            foreach (var filename in Directory.GetFiles("Resources/Shaders/", "*.glsl"))
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

            foreach (var filename in Directory.GetFiles("Resources/Textures/", "*.png"))
                resources.Textures.Load(filename, true, TextureTarget.Texture2D);

            camera = new Camera(Window) { Position = new Vector3d(10, 10, 10), Direction = new Vector3d(-1, -1, -1) };

            world_render_context.Window = Window;
            world_render_context.Camera = camera;

            Window.Keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(OnKeyUp);

            world = new World(resources);

            cursor = new Cursor(resources);
            Window.Mouse.ButtonUp += (s, e) =>
            {
                if (e.Button == MouseButton.Left)
                {
                    selection.ForEach(t => t.Mark = default(OpenTK.Graphics.Color4));
                    selection.Clear();
                    foreach (var hit in world.Intersect(ref cursor.Ray))
                    {
                        selection.Add((Physical)hit.Object1);
                        if (hit.Parent == null)
                            Console.WriteLine("{0}", hit.Object1);
                        else
                            Console.WriteLine("{0} > {1}", hit.Parent, hit.Object1);
                    }
                    Console.WriteLine();
                    selection.ForEach(t => t.Mark = new OpenTK.Graphics.Color4(255, 255, 0, 255));
                }
                if (e.Button == MouseButton.Right)
                    foreach (var entity in selection)
                        if (entity is INavigation)
                            (entity as INavigation).Target = cursor.Position;
            };
            world.AddChild(cursor);

            file_system = new FileSystem();
            savegame_filepath = "world.save";
            //if (File.Exists(savegame_filepath))
            //    using (var stream = new FileStream(savegame_filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
            //        if (stream.Length != 0)
            //        {
            //            world.Load(file_system.Load<SaveValue>(stream));
            //            return;
            //        }

            world.AddChild(new Grid(resources));
            world.AddChild(new Starfield(resources));
            var s0 = new Ship(resources) { Position = new Vector3d(5, 0, 0), Target = new Vector3d(5, 0, 0) };
            s0.Physic.AngularVelocity = Vector3d.UnitX;
            world.AddChild(s0);
            var s1 = new Ship(resources) { Position = new Vector3d(0, 0, 0), Target = new Vector3d(0, 0, 0) };
            s1.Physic.AngularVelocity = Vector3d.UnitY;
            world.AddChild(s1);
            var s2 = new Ship(resources) { Position = new Vector3d(-5, 0, 0), Target = new Vector3d(-5, 0, 0) };
            s2.Physic.AngularVelocity = Vector3d.UnitZ;
            world.AddChild(s2);
            var a0 = new Asteroid(resources) { Position = new Vector3d(0, 0, 5), Scale = new Vector3d(2) };
            a0.Physic.Velocity = Vector3d.Zero;
            a0.Physic.AngularVelocity = new Vector3d(0.25, 0.5, 0.75);
            world.AddChild(a0);
            var t0 = new Textured(resources) { Position = new Vector3d(3, -3, 0), Scale = new Vector3d(2) };
            t0.Physic.AngularVelocity = Vector3d.UnitY;
            world.AddChild(t0);
            var t1 = new Textured(resources) { Position = new Vector3d(-3, -4, 0), Scale = new Vector3d(2) };
            t1.Physic.AngularVelocity = -Vector3d.UnitY;
            world.AddChild(t1);
            world.AddChild(new Textured(resources) { Position = new Vector3d(0, -14, 0), Scale = new Vector3d(7) });
            world.AddChild(new Textured(resources) { Position = new Vector3d(-14, 0, 0), Scale = new Vector3d(7), Orientation = new Quaterniond(Vector3d.UnitZ, -1) });

            screen = new Screen(resources);
            screen.AddChild(new Entities.GUI.FPSCounter(resources));
            screen.AddChild(new Entities.GUI.Debugger(resources));

            var screen_camera = new Camera(Window);
            screen_render_context.Window = Window;
            screen_render_context.Camera = screen_camera;
            screen_render_context.Projection = screen_camera.ProjectionMatrix();
            screen_render_context.View = screen_camera.ViewMatrix();
            screen_render_context.Model = Matrix4d.Identity;

            var texture_target = TextureTarget.Texture2D;
            world_render_context.DepthTexture = new Texture(texture_target);
            using (new Bind(world_render_context.DepthTexture))
            {
                GL.TexImage2D(texture_target, 0, PixelInternalFormat.DepthComponent, 1024, 1024, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
                GL.TexParameter(texture_target, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(texture_target, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(texture_target, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(texture_target, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(texture_target, TextureParameterName.TextureCompareMode, (int)TextureCompareMode.CompareRToTexture);
                GL.TexParameter(texture_target, TextureParameterName.TextureCompareFunc, (int)DepthFunction.Lequal);
                //GL.TexParameter(texture_target, TextureParameterName.DepthTextureMode, (int)All.Intensity);
                //GL.TexParameter(texture_target, TextureParameterName.DepthTextureMode, (int)All.Luminance);
            }

            frame_buffer = new FrameBuffer(FramebufferTarget.Framebuffer);
            frame_buffer.Texture(FramebufferAttachment.DepthAttachment, world_render_context.DepthTexture, 0);
            frame_buffer.DrawBuffer(DrawBufferMode.None);
            frame_buffer.CheckStatus();
        }

        public override void Dispose()
        {
            file_system.Save(savegame_filepath, world.SaveValue());
            world.Dispose();
            selection.Clear();
            resources.Dispose();
            cursor.Dispose();
        }

        protected void OnKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.F3)
                world_render_context.Debug = !world_render_context.Debug;
            if (e.Key == Key.F10)
                camera.MouseLook = !camera.MouseLook;
            if (e.Key == Key.Plus)
            {
                Window.RequestContext();
                world.AddChild(new Asteroid(resources));
                Window.ReleaseContext();
            }
            if (e.Key == Key.Minus)
                foreach (var child in world.Children)
                    if (child is Asteroid)
                    {
                        Window.RequestContext();
                        world.RemoveChild(child);
                        Window.ReleaseContext();
                        break;
                    }
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
                    entity.Physic.AddForceRelative(new Vector3d(0, 0, -1), new Vector3d(10, 0, 0) * delta_time);
                    entity.Physic.AddForceRelative(new Vector3d(0, 0, 1), new Vector3d(-10, 0, 0) * delta_time);
                }
            if (Window.Keyboard[Key.L])
                foreach (var entity in selection)
                {
                    entity.Physic.AddForceRelative(new Vector3d(0, 0, -1), new Vector3d(-10, 0, 0) * delta_time);
                    entity.Physic.AddForceRelative(new Vector3d(0, 0, 1), new Vector3d(10, 0, 0) * delta_time);
                }
            if (Window.Keyboard[Key.I])
                foreach (var entity in selection)
                {
                    entity.Physic.AddForceRelative(new Vector3d(0, 0, -1), new Vector3d(0, 10, 0) * delta_time);
                    entity.Physic.AddForceRelative(new Vector3d(0, 0, 1), new Vector3d(0, -10, 0) * delta_time);
                }
            if (Window.Keyboard[Key.K])
                foreach (var entity in selection)
                {
                    entity.Physic.AddForceRelative(new Vector3d(0, 0, -1), new Vector3d(0, -10, 0) * delta_time);
                    entity.Physic.AddForceRelative(new Vector3d(0, 0, 1), new Vector3d(0, 10, 0) * delta_time);
                }
            if (Window.Keyboard[Key.U])
                foreach (var entity in selection)
                {
                    entity.Physic.AddForceRelative(new Vector3d(-1, 0, 0), new Vector3d(0, -10, 0) * delta_time);
                    entity.Physic.AddForceRelative(new Vector3d(1, 0, 0), new Vector3d(0, 10, 0) * delta_time);
                }
            if (Window.Keyboard[Key.O])
                foreach (var entity in selection)
                {
                    entity.Physic.AddForceRelative(new Vector3d(-1, 0, 0), new Vector3d(0, 10, 0) * delta_time);
                    entity.Physic.AddForceRelative(new Vector3d(1, 0, 0), new Vector3d(0, -10, 0) * delta_time);
                }
            if (Window.Keyboard[Key.M])
                foreach (var entity in selection)
                    entity.Physic.AngularVelocity = Vector3d.Zero;

            camera.Update(delta_time);
            world_render_context.Projection = camera.ProjectionMatrix();
            world_render_context.View = camera.ViewMatrix();
            world_render_context.Model = Matrix4d.Identity;

            world.Update(delta_time, world_render_context);

            screen.Update(delta_time, screen_render_context);
        }

        FrameBuffer frame_buffer;

        public override void Render(double delta_time)
        {
            #region Depth
            var light = new Vector3d(10, 10, 0);
            var light_target = new Vector3d(0, 0, 0);
            var depth_render_context = new RenderContext()
            {
                Window = Window,
                Projection = Matrix4d.CreateOrthographicOffCenter(-10, 10, -10, 10, 0, 30),
                View = Matrix4d.LookAt(light, light_target, Vector3d.UnitY),
                Model = Matrix4d.Identity,
                Program = resources.Programs["depth"]
            };
            var depth_bias = Matrix4d.Scale(0.5, 0.5, 0.5) * Matrix4d.CreateTranslation(0.5, 0.5, 0.5); // Map [-1, 1] to [0, 1]
            var depth_bias_MVP = world_render_context.InvertedView * depth_render_context.ProjectionViewModel * depth_bias;
            resources.Programs["standard"].UniformLocations["in_DepthBiasMVP"].Set(depth_bias_MVP);
            resources.Programs["standard"].UniformLocations["in_Texture"].Set(0);
            resources.Programs["standard"].UniformLocations["in_ShadowTexture"].Set(1);
            resources.Programs["standard"].UniformLocations["in_LightPosition"].Set(light);
            //GL.Enable(EnableCap.PolygonOffsetFill);
            //GL.PolygonOffset(2.0f, 4.0f);
            //GL.CullFace(CullFaceMode.Front);
            using (new Bind(frame_buffer))
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.Viewport(0, 0, 1024, 1024);
                world.Render(delta_time, depth_render_context);
            }
            //GL.CullFace(CullFaceMode.Back);
            //GL.Disable(EnableCap.PolygonOffsetFill);
            #endregion

            //#region Shadow
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //GL.Viewport(0, 0, Window.Width, Window.Height);
            //using (new Bind(resources.Textures["Resources/Textures/debug-256.png"]))
            //using (new Bind(world_render_context.DepthTexture))
            //    world.Render(delta_time, world_render_context);
            //#endregion

            #region Accumulation
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Window.Width, Window.Height);

            world_render_context.alpha_cutoff = 0.35f;
            using (new Bind(resources.Textures["Resources/Textures/debug-256.png"]))
            using (new Bind(world_render_context.DepthTexture))
                world.Render(delta_time, world_render_context);
            GL.Accum(AccumOp.Load, 0.5f);

            world_render_context.alpha_cutoff = 0.65f;
            using (new Bind(resources.Textures["Resources/Textures/debug-256.png"]))
            using (new Bind(world_render_context.DepthTexture))
                world.Render(delta_time, world_render_context);
            GL.Accum(AccumOp.Accum, 0.5f);
            GL.Accum(AccumOp.Return, 1f);

            screen.Render(delta_time, screen_render_context);
            #endregion

            #region Debug
            GL.Viewport(0, 0, Window.Width, Window.Height);
            var debug_render_context = new RenderContext()
            {
                Window = Window,
                Projection = Matrix4d.CreateOrthographicOffCenter(-1, 1, -1, 1, -1, 1),
                View = Matrix4d.Identity,
                Model = Matrix4d.Scale(0.25) * Matrix4d.CreateTranslation(-0.75, -0.75, 0)
            };
            var debug_screen = new MartinZottmann.Engine.Graphics.OpenGL.Entity();
            var shape = new MartinZottmann.Engine.Graphics.Shapes.Quad();
            shape.Vertices[0].Texcoord.Y = 0;
            shape.Vertices[1].Texcoord.Y = 0;
            shape.Vertices[2].Texcoord.Y = 1;
            shape.Vertices[3].Texcoord.Y = 1;
            debug_screen.Mesh(shape);
            debug_screen.Program = resources.Programs["plain_texture"];
            debug_screen.Program.UniformLocations["in_ModelViewProjection"].Set(debug_render_context.ProjectionViewModel);
            debug_screen.Program.UniformLocations["in_Texture"].Set(0);
            debug_screen.Texture = world_render_context.DepthTexture;
            debug_screen.Draw();
            #endregion

            Window.SwapBuffers();
        }
    }
}
