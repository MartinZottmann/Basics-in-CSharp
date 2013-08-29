using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Physics;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Entities.Nodes;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace MartinZottmann.Game.Entities.Systems
{
    public class CursorSystem : ISystem
    {
        public Camera Camera;

        public GameWindow Window;

        protected NodeList<CursorNode> cursor_nodes;

        protected NodeList<PhysicNode> physic_nodes;

        public Ray3d Ray;

        public Plane3d Plane = new Plane3d(Vector3d.Zero, Vector3d.UnitY);

        protected List<PhysicNode> selection = new List<PhysicNode>();

        public CursorSystem(GameWindow window, Camera camera)
        {
            Window = window;
            Camera = camera;

            Window.Mouse.ButtonUp += OnButtonUp;
        }

        public void Bind(EntityManager entitiy_manager)
        {
            cursor_nodes = entitiy_manager.Get<CursorNode>();
            physic_nodes = entitiy_manager.Get<PhysicNode>();
        }

        public void Update(double delta_time)
        {
            foreach (var cursor_node in cursor_nodes)
                SetCursor(cursor_node);
        }

        public void Render(double delta_time) { }

        public void SetCursor(CursorNode input_node)
        {
            Ray = Camera.GetMouseRay();
            Ray.Intersect(Plane, out input_node.Base.Position);
        }

        protected void OnButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                selection.ForEach(t => t.Base.Mark = OpenTK.Graphics.Color4.Pink);
                selection.Clear();
                foreach (var cursor_node in cursor_nodes)
                    foreach (var hit in Intersections())
                    {
                        selection.Add((PhysicNode)hit.Object1);
                        Console.WriteLine("Select: {0}", hit.Object1);
                    }
                selection.ForEach(t => t.Base.Mark = new OpenTK.Graphics.Color4(255, 255, 0, 255));
            }
            if (e.Button == MouseButton.Right)
                foreach (var cursor_node in cursor_nodes)
                    foreach (var physic_node in selection)
                        if (physic_node.Entity.Has<TargetComponent>())
                            physic_node.Entity.Get<TargetComponent>().Position = cursor_node.Base.Position;
        }

        protected SortedSet<Collision> Intersections()
        {
            var hits = new SortedSet<Collision>();

            foreach (var physic_node in physic_nodes)
            {
                var hit = physic_node.Physic.BoundingBox.At(physic_node.Base.WorldPosition).Collides(ref Ray);
                if (hit == null)
                    continue;

                hit.Object0 = Ray;
                hit.Object1 = physic_node;
                hits.Add(hit);
            }

            return hits;
        }
    }
}
