using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using OpenTK;
using System;
using System.Drawing;

namespace MartinZottmann.Game.Entities.GUI
{
    public class Debugger : Abstract
    {
        protected SizeF size = new SizeF(300, 30);

        protected ResourceManager resource_manager;

        public override void Start(ResourceManager resource_manager)
        {
            base.Start(resource_manager);

            Scale = new Vector3d(0.75);
            Position = new Vector3d(0.1, 0.8, 0.0);
            this.resource_manager = resource_manager;
        }

        public override void Update(double delta_time)
        {
            Model.Mesh(
                FontMeshBuilder.FromString(
                    String.Format(
                        "Memory: {0}\nWorld objects: {1}\nEntities: {2}\nPrograms: {3}\nShaders: {4}\nTextures: {5}\nWavefrontObjFiles: {6}",
                        GC.GetTotalMemory(false),
                        "???",
                        resource_manager.Models.Count,
                        resource_manager.Programs.Count,
                        resource_manager.Shaders.Count,
                        resource_manager.Textures.Count,
                        resource_manager.WavefrontObjFiles.Count
                    )
                )
            );
        }
    }
}
