using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Graphics;
using OpenTK;
using System;
using System.Drawing;

namespace MartinZottmann.Game.Entities.GUI
{
    public class FPSCounter : Abstract
    {
        protected double accumulator = 0;

        protected int counter = 0;

        protected int fps = 0;

        protected SizeF size = new SizeF(300, 30);

        public override void Start(ResourceManager resource_manager)
        {
            base.Start(resource_manager);

            Scale = new Vector3d(2);
            Position = new Vector3d(-0.90, 0.75, -1);
            Model.Mesh(FontMeshBuilder.FromString(String.Format("FPS: {0:F}", counter)));
        }

        public override void Update(double delta_time)
        {
            counter++;
            accumulator += delta_time;
            if (accumulator > 1)
            {
                if (fps != counter)
                    Model.Mesh(FontMeshBuilder.FromString(String.Format("FPS: {0:F}", counter)));
                fps = counter;
                counter = 0;
                accumulator -= 1;
            }
        }
    }
}
