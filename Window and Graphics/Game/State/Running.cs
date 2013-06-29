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

            for (int i = 1; i <= 5; i++)
                Add(
                    new Ship(resources)
                    {
                        Position = new Vector3d(
                            (MartinZottmann.Game.Entities.Entity.Random.NextDouble() - 0.5) * 100.0,
                            0.0,
                            (MartinZottmann.Game.Entities.Entity.Random.NextDouble() - 0.5) * 100.0
                        )
                    }
                );

            //Add(new Explosion(resources));
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
                if (entity is Ship)
                    if ((entity as Ship).Velocity.LengthSquared < 0.01)
                        (entity as Ship).Target = new Vector3d(
                            (MartinZottmann.Game.Entities.Entity.Random.NextDouble() - 0.5) * 100.0,
                            0.0,
                            (MartinZottmann.Game.Entities.Entity.Random.NextDouble() - 0.5) * 100.0
                        );

            var collisions = DetectCollisions();

            foreach (Entities.Entity entity in entities)
                if (entity is Physical)
                    (entity as Physical).UpdateVelocity(delta_time);

            ApplyImpusles(collisions, delta_time);

            foreach (Entities.Entity entity in entities)
                if (entity is Physical)
                    (entity as Physical).UpdatePosition(delta_time);

            foreach (Entities.Entity entity in entities)
            {
                entity.Update(delta_time, render_context);

                entity.Reposition(100, 100, 100);
            }
        }

        protected List<Collision> DetectCollisions()
        {
            var collisions = new List<Collision>();

            foreach (Entities.Entity a in entities)
            {
                if (!(a is Physical))
                    continue;

                foreach (Entities.Entity b in entities)
                {
                    if (a == b)
                        continue;

                    if (!(b is Physical))
                        continue;

                    Vector3d hit_a;
                    Vector3d hit_b;
                    double penetration_depth;

                    var i = a as Physical;
                    var j = b as Physical;

                    if (!i.BoundingBox.Intersect(ref i.Position, ref j.BoundingBox, ref j.Position))
                        continue;

                    var s = i.BoundingSphere;
                    s.Origin += i.Position;
                    var t = j.BoundingSphere;
                    t.Origin += j.Position;

                    if (s.Intersect(ref t, out hit_a, out hit_b, out penetration_depth))
                        collisions.Add(
                            new Collision()
                            {
                                HitPoint = i.Position + hit_a,
                                Normal = (i.Position - j.Position).Normalized() * penetration_depth,
                                Object0 = i,
                                Object1 = j,
                                PenetrationDepth = penetration_depth
                            }
                        );
                }
            }

            return collisions;
        }

        protected void ApplyImpusles(List<Collision> collisions, double delta_time)
        {
            foreach (var collision in collisions)
            {
                var r0 = collision.HitPoint - collision.Object0.Position;
                var r1 = collision.HitPoint - collision.Object1.Position;
                var v0 = collision.Object0.Velocity + Vector3d.Cross(collision.Object0.AngularVelocity, r0);
                var v1 = collision.Object1.Velocity + Vector3d.Cross(collision.Object1.AngularVelocity, r1);
                var dv = v0 - v1;

                if (-Vector3d.Dot(dv, collision.Normal) < -0.01)
                    return;

                #region NORMAL Impulse
                var e = 0.0;
                var normDiv = Vector3d.Dot(collision.Normal, collision.Normal) * (
                    (collision.Object0.InverseMass + collision.Object1.InverseMass)
                    + Vector3d.Dot(
                        collision.Normal,
                        Vector3d.Cross(Vector3d.Cross(r0, collision.Normal) * collision.Object0.InverseInertiaWorld, r0)
                        + Vector3d.Cross(Vector3d.Cross(r1, collision.Normal) * collision.Object1.InverseInertiaWorld, r1)
                    )
                );
                var jn = -1 * (1 + e) * Vector3d.Dot(dv, collision.Normal) / normDiv;
                jn += (collision.PenetrationDepth * 1.5);
                var Pn = collision.Normal * jn;

                collision.Object0.AddImpulse(r0, Pn);
                collision.Object1.AddImpulse(r1, -1 * Pn);
                #endregion

                #region TANGENT Impulse
                var tangent = dv - (Vector3d.Dot(dv, collision.Normal) * collision.Normal);
                tangent.Normalize();
                var k_tangent = collision.Object0.InverseMass
                    + collision.Object1.InverseMass
                    + Vector3d.Dot(
                        tangent,
                        Vector3d.Cross(Vector3d.Cross(r0, tangent) * collision.Object0.InverseInertiaWorld, r0)
                        + Vector3d.Cross(Vector3d.Cross(r1, tangent) * collision.Object1.InverseInertiaWorld, r1)
                    );
                var Pt = -1 * Vector3d.Dot(dv, tangent) / k_tangent * tangent;

                collision.Object0.AddImpulse(r0, Pt);
                collision.Object1.AddImpulse(r1, -1 * Pt);
                #endregion
            }
        }

        public override void Render(double delta_time)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Window.Width, Window.Height);

            foreach (Entities.Entity entity in entities)
                entity.Render(delta_time, render_context);

            Window.SwapBuffers();
        }
    }

    public struct Collision
    {
        public Vector3d HitPoint;

        public Vector3d Normal;

        public Physical Object0;

        public Physical Object1;

        public double PenetrationDepth;
    }
}
