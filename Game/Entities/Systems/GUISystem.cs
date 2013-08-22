﻿using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.GUI;
using MartinZottmann.Game.Graphics;
using OpenTK;
using System.Collections.Generic;
using System.Drawing;

namespace MartinZottmann.Game.Entities.Systems
{
    public class GUISystem : ISystem
    {
        public MartinZottmann.Game.Graphics.RenderContext RenderContext = new MartinZottmann.Game.Graphics.RenderContext();

        public Camera Camera;

        public ResourceManager ResourceManager;

        protected List<IGUIElement> gui_elements = new List<IGUIElement>();

        public GUISystem(Camera camera, ResourceManager resource_manager)
        {
            Camera = camera;
            ResourceManager = resource_manager;

            FontStructure font_map;
            var font_texture = new Texture(new Font("Arial", 20, FontStyle.Regular, GraphicsUnit.Pixel, (byte)0), Color.White, Color.Black, Color.Transparent, false, out font_map);
            var font_mesh_builder = new FontMeshBuilder(font_map);

            // @todo Move hardcoded GUI elements
            gui_elements.Add(new Debugger());
            gui_elements.Add(new FPSCounter());

            foreach (var gui_element in gui_elements)
            {
                gui_element.FontTexture = font_texture;
                gui_element.FontMeshBuilder = font_mesh_builder;
                gui_element.Start(resource_manager);
            }
        }

        public void Bind(EntityManager entitiy_manager)
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
                gui_element.Model.Program.UniformLocations["in_Texture"].Set(0);
                gui_element.Render(delta_time);
            }
        }
    }
}
