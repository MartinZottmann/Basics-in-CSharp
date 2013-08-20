using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Graphics;
using OpenTK;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MartinZottmann.Game.Entities.GUI
{
    public class Debugger : Abstract
    {
        protected SizeF size = new SizeF(300, 30);

        public ResourceManager ResourceManager;

        public override void Start(ResourceManager resource_manager)
        {
            base.Start(resource_manager);

            Scale = new Vector3d(1500.0);
            Position = new Vector3d(0.1, 0.8, 0.0);
            ResourceManager = resource_manager;
        }

        public override void Update(double delta_time)
        {
            Model.Mesh(
                FontMeshBuilder.FromString(
                    String.Format(
                        "Memory: {0}\nWorld objects: {1}\nEntities: {2}\nPrograms: {3}\nShaders: {4}\nTextures: {5}\nWavefrontObjFiles: {6}",
                        GC.GetTotalMemory(false),
                        "???",
                        ResourceManager.Entities.Count,
                        ResourceManager.Programs.Count,
                        ResourceManager.Shaders.Count,
                        ResourceManager.Textures.Count,
                        ResourceManager.WavefrontObjFiles.Count
                    )
                )
            );
        }
    }
}
