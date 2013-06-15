using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities;
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

        protected Physical steerable;

        protected Camera camera;

        protected Resources resources;

        public Running(GameWindow window)
            : base(window)
        {
            resources = new Resources();
            var vertex_shader = resources.Shaders.Load(ShaderType.VertexShader, "res/Shaders/standard.vs.glsl");
            var fragment_shader = resources.Shaders.Load(ShaderType.FragmentShader, "res/Shaders/standard.fs.glsl");
            resources.Programs.Load(
                "standard",
                new Shader[] {
                    vertex_shader,
                    fragment_shader
                },
                new string[] {
                    "in_Position",
                    "in_Normal",
                    "in_Texcoord"
                }
            );
            vertex_shader = resources.Shaders.Load(ShaderType.VertexShader, "res/Shaders/phong.vs.glsl");
            fragment_shader = resources.Shaders.Load(ShaderType.FragmentShader, "res/Shaders/phong.fs.glsl");
            resources.Programs.Load(
                "phong",
                new Shader[] {
                    vertex_shader,
                    fragment_shader
                },
                new string[] {
                    "in_Position",
                    "in_Normal",
                    "in_Texcoord"
                }
            );
            vertex_shader = resources.Shaders.Load(ShaderType.VertexShader, "res/Shaders/normal.vs.glsl");
            fragment_shader = resources.Shaders.Load(ShaderType.FragmentShader, "res/Shaders/normal.fs.glsl");
            resources.Programs.Load(
                "normal",
                new Shader[] {
                    vertex_shader,
                    fragment_shader
                },
                new string[] {
                    "in_Position",
                    "in_Color"
                }
            );
            resources.Programs["normal"].AddUniformLocation("PVM");
            vertex_shader = resources.Shaders.Load(ShaderType.VertexShader, "res/Shaders/plain_texture.vs.glsl");
            fragment_shader = resources.Shaders.Load(ShaderType.FragmentShader, "res/Shaders/plain_texture.fs.glsl");
            resources.Programs.Load(
                "plain_texture",
                new Shader[] {
                    vertex_shader,
                    fragment_shader
                },
                new string[] {
                    "in_Position",
                    "in_Normal",
                    "in_Texcoord"
                }
            );
            resources.Programs["plain_texture"].AddUniformLocation("PVM");
            resources.Programs["plain_texture"].AddUniformLocation("Texture");

            foreach (var filename in Directory.GetFiles("res/textures/", "*.png"))
            {
                resources.Textures.Load(filename, true, TextureTarget.Texture2D);
            }

            camera = new Camera(Window);
            camera.MouseLook = true;
            camera.Position.X = 10;
            camera.Position.Y = 10;
            camera.Position.Z = 100;

            Window.Keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(OnKeyUp);

            Add(new Entities.GUI.FPSCounter(resources));

            Add(new Grid(resources));

            Add(new Starfield(resources));

            for (int i = 1; i <= 10; i++)
                Add(new Asteroid(resources));

            var textured = new Textured(resources);
            steerable = textured;
            textured.quad[0] = new Vector3d(-10, 0, -10);
            textured.quad[1] = new Vector3d(-10, 0, 10);
            textured.quad[2] = new Vector3d(10, 0, 10);
            textured.quad[3] = new Vector3d(10, 0, -10);
            Add(textured);

            for (int i = 1; i <= 10; i++)
                Add(new SuperBall(resources));
        }

        public override void Dispose()
        {
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
                camera.Position -= camera.Direction * delta_time * 100;
            //steerable.Velocity.Y += 100 * delta_time;
            if (Window.Keyboard[Key.S])
                camera.Position += camera.Direction * delta_time * 100;
            //steerable.Velocity.Y -= 100 * delta_time;
            if (Window.Keyboard[Key.A])
                camera.Position += camera.Right * delta_time * 100;
            //steerable.Velocity.X -= 100 * delta_time;
            if (Window.Keyboard[Key.D])
                camera.Position -= camera.Right * delta_time * 100;
            //steerable.Velocity.X += 100 * delta_time;
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

            camera.Update(delta_time);

            foreach (Entities.Entity entity in entities)
            {
                entity.Update(delta_time);

                entity.Reposition(100, 100, 100);
            }
        }

        public override void Render(double delta_time)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Window.Width, Window.Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            {
                var projection_matrix = camera.ProjectionMatrix();
                //GL.LoadMatrix(ref projection_matrix);

                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();
                {
                    var view_matrix = camera.ViewMatrix();
                    //GL.LoadMatrix(ref view_matrix);

                    var render_context = new RenderContext()
                    {
                        Camera = camera,
                        Projection = projection_matrix,
                        View = view_matrix
                    };

                    foreach (Entities.Entity entity in entities)
                    {
                        entity.RenderContext = render_context;
                        entity.Render(delta_time);
                    }
                }
                GL.MatrixMode(MatrixMode.Modelview);
                GL.PopMatrix();
            }
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();

            Window.SwapBuffers();
        }
    }
}
