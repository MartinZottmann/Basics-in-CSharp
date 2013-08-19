using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.GUI;
using MartinZottmann.Game.Graphics;
using System.Collections.Generic;
using System.Drawing;

namespace MartinZottmann.Game.Entities.Systems
{
    public class GUISystem : ISystem
    {
        public MartinZottmann.Game.Graphics.RenderContext RenderContext = new MartinZottmann.Game.Graphics.RenderContext();

        public Camera Camera;

        public ResourceManager ResourceManager;

        protected List<IGUIElement> list = new List<IGUIElement>();

        public GUISystem(Camera camera, ResourceManager resource_manager)
        {
            Camera = camera;
            ResourceManager = resource_manager;

            FontStructure font_map;
            var font_texture = new Texture(new Font("Arial", 20, FontStyle.Regular, GraphicsUnit.Pixel, (byte)0), Color.White, Color.Black, Color.Transparent, false, out font_map);
            var font_mesh_builder = new FontMeshBuilder(font_map);

            // @todo Move hardcoded GUI elements
            list.Add(new Debugger());
            list.Add(new FPSCounter());

            foreach (var i in list)
            {
                i.FontTexture = font_texture;
                i.FontMeshBuilder = font_mesh_builder;
                i.Start(resource_manager);
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

            foreach (var i in list)
                i.Update(delta_time);
        }

        public void Render(double delta_time)
        {
            foreach (var i in list)
            {
                RenderContext.Model = i.ModelMatrix * RenderContext.InvertedProjection;
                i.Model.Program.UniformLocations["in_ModelViewProjection"].Set(RenderContext.ProjectionViewModel);
                i.Render(delta_time);
            }
        }
    }
}
