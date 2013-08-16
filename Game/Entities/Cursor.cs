using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Physics;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Graphics;
using MartinZottmann.Game.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace MartinZottmann.Game.Entities
{
    public class Cursor : GameObject
    {
        public Ray3d Ray;

        public Plane3d Plane = new Plane3d(Vector3d.Zero, Vector3d.UnitY);

        public Cursor(ResourceManager resources)
            : base(resources)
        {
            var graphic = AddComponent(new Graphic(this));
            graphic.Model = new Engine.Graphics.OpenGL.Entity();
            graphic.Model.Mesh(
                new Mesh<VertexP3C4, uint>(
                    new VertexP3C4[] {
                        new VertexP3C4(-1, 0, 0, 1, 1, 1, 1),
                        new VertexP3C4(0, 0, 1, 1, 0, 1, 1),
                        new VertexP3C4(1, 0, 0, 1, 1, 1, 1),
                        new VertexP3C4(0, 0, -1, 0, 1, 1, 1)
                    },
                    new uint[] { 0, 1, 2, 3 }
                )
            );
            graphic.Model.Mode = BeginMode.LineLoop;
            graphic.Model.Program = Resources.Programs["normal"];
        }

        public void Set(RenderContext render_context)
        {
            var start = new Vector4d(
                (render_context.Window.Mouse.X / (double)render_context.Window.Width - 0.5) * 2.0,
                ((render_context.Window.Height - render_context.Window.Mouse.Y) / (double)render_context.Window.Height - 0.5) * 2.0,
                -1.0,
                1.0
            );
            var end = new Vector4d(
                (render_context.Window.Mouse.X / (double)render_context.Window.Width - 0.5) * 2.0,
                ((render_context.Window.Height - render_context.Window.Mouse.Y) / (double)render_context.Window.Height - 0.5) * 2.0,
                0,
                1.0
            );

            var IP = render_context.InvertedProjection;
            start = Vector4d.Transform(start, IP);
            start /= start.W;
            end = Vector4d.Transform(end, IP);
            end /= end.W;

            var IV = render_context.InvertedView;
            start = Vector4d.Transform(start, IV);
            start /= start.W;
            end = Vector4d.Transform(end, IV);
            end /= end.W;

            end = end - start;
            end.W = 1;

            Ray = new Ray3d(
                new Vector3d(start.X, start.Y, start.Z),
                Vector3d.Normalize(new Vector3d(end.X, end.Y, end.Z))
            );

            Ray.Intersect(Plane, out Position);
        }

        public virtual SortedSet<Collision> Intersect(ref Ray3d ray, ref GameObject world)
        {
            var world_orientation_matrix = Matrix4d.Identity; // world.GetComponent<Physic>().OrientationMatrix
            var hits = new SortedSet<Collision>();

            foreach (var child in world.GetComponent<Children>())
                if (child.HasComponent<Physic>())
                    foreach (var hit in child.GetComponent<Physic>().Intersect(ref ray, ref world_orientation_matrix))
                        hits.Add(hit);

            return hits;
        }

        public override void Update(double delta_time, RenderContext render_context)
        {
            Set(render_context);

            base.Update(delta_time, render_context);
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            render_context.Model = Model;

            GL.LineWidth(1);
            base.Render(delta_time, render_context);
        }

        public override SaveValue SaveValue()
        {
            return null;
        }
    }
}
