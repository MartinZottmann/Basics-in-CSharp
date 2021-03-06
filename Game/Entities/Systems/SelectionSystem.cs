﻿using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Physics;
using MartinZottmann.Game.Entities.Nodes;
using MartinZottmann.Game.Input;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace MartinZottmann.Game.Entities.Systems
{
    public class SelectionSystem : ISystem
    {
        public readonly InputManager InputManager;

        public readonly Camera Camera;

        protected NodeList<PhysicNode> physic_nodes;

        protected List<PhysicNode> selection = new List<PhysicNode>();

        public SelectionSystem(InputManager input_manager, Camera camera)
        {
            InputManager = input_manager;
            InputManager.ButtonUp += OnButtonUp;

            Camera = camera;
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

        protected void OnButtonUp(object sender, InputMouseEventArgs e)
        {
            if (e.Handled)
                return;

            if (e.Button == MouseButton.Left)
            {
                Select();
                if (0 != selection.Count)
                    e.Handled = true;
            }
        }

        protected void Select()
        {
            selection.ForEach(t => t.Base.Mark = OpenTK.Graphics.Color4.Pink);
            selection.Clear();
            foreach (var hit in Intersections())
            {
                selection.Add((PhysicNode)hit.Value.Object1);
                Console.WriteLine("Select: {0}, {1}", hit.Key, hit.Value.Object1);
            }
            selection.ForEach(t => t.Base.Mark = new OpenTK.Graphics.Color4(255, 255, 0, 255));
        }

        protected SortedList<double, Collision> Intersections()
        {
            var ray = Camera.GetMouseRay();
            var hits = new SortedList<double, Collision>();

            foreach (var physic_node in physic_nodes)
            {
                var hit = physic_node.Physic.BoundingBox.At(physic_node.Base.WorldPosition).Collides(ref ray);
                if (null == hit)
                    continue;

                hit.Object0 = ray;
                hit.Object1 = physic_node;
                hits.Add((physic_node.Base.Position - ray.Origin).Length, hit);
            }

            return hits;
        }
    }
}
