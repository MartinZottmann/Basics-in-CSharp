using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using OpenTK;
using System;

namespace MartinZottmann.Game.Entities.GUI
{
    public class FPSCounter : Abstract
    {
        public class Counter
        {
            protected double accumulator = 0;

            protected int counter = 0;

            protected int fps = 0;

            public int FPS { get { return fps; } }

            public void Tick(double delta_time)
            {
                counter++;
                accumulator += delta_time;
                if (accumulator > 1)
                {
                    fps = counter;
                    counter = 0;
                    accumulator -= 1;
                }
            }
        }

        protected Counter update = new Counter();

        protected Counter render = new Counter();

        public override void Bind(ResourceManager resource_manager, FontMeshBuilder font_mesh_builder)
        {
            base.Bind(resource_manager, font_mesh_builder);

            Scale = new Vector3d(0.75);
            Position = new Vector3d(0.1, 0.9, 0.0);
            RebuildModel();
        }

        public override void Update(double delta_time)
        {
            update.Tick(delta_time);
            base.Update(delta_time);
        }

        public override void Render(double delta_time)
        {
            render.Tick(delta_time);
            RebuildModel();
            base.Render(delta_time);
        }

        protected void RebuildModel()
        {
            Model.Mesh(font_mesh_builder.FromString(String.Format("FPS: {0:F}/{1:F}", update.FPS, render.FPS)));
        }
    }
}
