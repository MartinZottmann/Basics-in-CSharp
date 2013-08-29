using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using OpenTK;
using System;

namespace MartinZottmann.Game.Entities.GUI
{
    public class FPSCounter : Abstract
    {
        protected double accumulator = 0;

        protected int counter = 0;

        protected int fps = 0;

        public override void Bind(ResourceManager resource_manager, FontMeshBuilder font_mesh_builder)
        {
            base.Bind(resource_manager, font_mesh_builder);

            Scale = new Vector3d(0.75);
            Position = new Vector3d(0.1, 0.9, 0.0);
            RebuildModel();
        }

        public override void Update(double delta_time)
        {
            counter++;
            accumulator += delta_time;
            if (accumulator > 1)
            {
                if (fps != counter)
                    RebuildModel();
                fps = counter;
                counter = 0;
                accumulator -= 1;
            }
        }

        protected void RebuildModel()
        {
            Model.Mesh(font_mesh_builder.FromString(String.Format("FPS: {0:F}", counter)));
        }
    }
}
