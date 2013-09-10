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

        protected NodeList<PhysicNode> physic_nodes;

        protected List<PhysicNode> selection = new List<PhysicNode>();

        public CursorSystem(GameWindow window, Camera camera)
        {
            Window = window;
            Camera = camera;

            Window.Mouse.ButtonUp += OnButtonUp;
        }

        public void Start(EntityManager entity_manager)
        {
            physic_nodes = entity_manager.GetNodeList<PhysicNode>();
        }

        public void Update(double delta_time) { }

        public void Render(double delta_time) { }

        public void Stop()
        {
            physic_nodes = null;
        }

        protected void OnButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                selection.ForEach(t => t.Base.Mark = OpenTK.Graphics.Color4.Pink);
                selection.Clear();
                foreach (var hit in Intersections())
                {
                    selection.Add((PhysicNode)hit.Object1);
                    Console.WriteLine("Select: {0}", hit.Object1);
                }
                selection.ForEach(t => t.Base.Mark = new OpenTK.Graphics.Color4(255, 255, 0, 255));
            }
            //if (e.Button == MouseButton.Right){
            //    foreach (var physic_node in selection)
            //        if (physic_node.Entity.Has<TargetComponent>())
            //            physic_node.Entity.Get<TargetComponent>().Position = cursor_node.Base.Position;}
        }

        protected SortedSet<Collision> Intersections()
        {
            var ray = Camera.GetMouseRay();
            var hits = new SortedSet<Collision>();

            foreach (var physic_node in physic_nodes)
            {
                var hit = physic_node.Physic.BoundingBox.At(physic_node.Base.WorldPosition).Collides(ref ray);
                if (null == hit)
                    continue;

                hit.Object0 = ray;
                hit.Object1 = physic_node;
                hits.Add(hit);
            }

            return hits;
        }
    }
}
