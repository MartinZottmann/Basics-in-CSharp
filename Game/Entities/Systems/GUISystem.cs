using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Physics;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.GUI;
using MartinZottmann.Game.Graphics;
using MartinZottmann.Game.Input;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MartinZottmann.Game.Entities.Systems
{
    public class GUISystem : ISystem
    {
        public MartinZottmann.Game.Graphics.RenderContext RenderContext = new MartinZottmann.Game.Graphics.RenderContext();

        public readonly Camera Camera;

        public readonly ResourceManager ResourceManager;

        protected FontMeshBuilder font_mesh_builder;

        protected List<IGUIElement> gui_elements = new List<IGUIElement>();

        public GUISystem(InputManager input_manager, Camera camera, ResourceManager resource_manager)
        {
            input_manager.ButtonUp += OnButtonUp;
            Camera = camera;
            ResourceManager = resource_manager;

            FontStructure font_map;
            var font_texture = new Texture(new Font("Arial", 20, FontStyle.Regular, GraphicsUnit.Pixel, (byte)0), Color.White, Color.Black, Color.Transparent, false, out font_map);
            resource_manager.Textures["GUI_font"] = font_texture;
            font_mesh_builder = new FontMeshBuilder(font_map);
        }

        public void Start(EntityManager entity_manager)
        {
            // @todo
        }

        public void Update(double delta_time)
        {
            // @todo
            RenderContext.Projection = Camera.ProjectionMatrix;
            RenderContext.View = Camera.ViewMatrix;

            foreach (var gui_element in gui_elements)
                gui_element.Update(delta_time);
        }

        public void Render(double delta_time)
        {
            foreach (var gui_element in gui_elements)
            {
                RenderContext.Model = Matrix4d.Scale(1.0 / Camera.Window.ClientRectangle.Width, 1.0 / Camera.Window.ClientRectangle.Height, 1.0)
                    * gui_element.ModelMatrix
                    * Matrix4d.CreateTranslation(-0.5, -0.5, 0.0)
                    * Matrix4d.Scale(2.0, 2.0, 1.0)
                    * RenderContext.InvertedProjection;

                gui_element.Model.Program.UniformLocations["in_ModelViewProjection"].Set(RenderContext.ProjectionViewModel);
                gui_element.Render(delta_time);
            }
        }

        public void Stop()
        {
            // @todo
        }

        public void Add(IGUIElement gui_element)
        {
            gui_elements.Add(gui_element);
            gui_element.Bind(ResourceManager, font_mesh_builder);
        }

        public void Remove(IGUIElement gui_element)
        {
            gui_elements.Remove(gui_element);
        }

        protected void OnButtonUp(object sender, InputMouseEventArgs e)
        {
            if (e.Handled)
                return;

            if (e.Button == MouseButton.Left)
                foreach (var hit in Intersections())
                {
                    Console.WriteLine("GUI Select: {0}", (IGUIElement)hit.Object1);
                    e.Handled = true;
                }
        }

        protected SortedSet<Collision> Intersections()
        {
            var ray = Camera.GetMouseRay();
            var hits = new SortedSet<Collision>();

            foreach (var gui_element in gui_elements)
            {
                var model_matrix = Matrix4d.Scale(1.0 / Camera.Window.ClientRectangle.Width, 1.0 / Camera.Window.ClientRectangle.Height, 1.0)
                    * gui_element.ModelMatrix
                    * Matrix4d.CreateTranslation(-0.5, -0.5, 0.0)
                    * Matrix4d.Scale(2.0, 2.0, 1.0)
                    * RenderContext.InvertedProjection;

                var hit = gui_element.Model.Mesh().BoundingBox.At(ref model_matrix).Collides(ref ray);
                if (null == hit)
                    continue;

                hit.Object0 = ray;
                hit.Object1 = gui_element;
                hits.Add(hit);
            }

            return hits;
        }
    }
}
