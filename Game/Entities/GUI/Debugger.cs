using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using OpenTK;
using System;

namespace MartinZottmann.Game.Entities.GUI
{
    public class Debugger : Abstract
    {
        protected EntityManager entity_manager;

        public Debugger(EntityManager entity_manager)
        {
            this.entity_manager = entity_manager;
        }

        public override void Bind(ResourceManager resource_manager, FontMeshBuilder font_mesh_builder)
        {
            base.Bind(resource_manager, font_mesh_builder);

            Scale = new Vector3d(0.75);
            Position = new Vector3d(0.1, 0.8, 0.0);
        }

        public override void Update(double delta_time)
        {
            Model.Mesh(
                font_mesh_builder.FromString(
                    String.Format(
                        "Memory: {0}\nWorld objects: {1}\nModels: {2}\nPrograms: {3}\nShaders: {4}\nTextures: {5}\nWavefrontObjFiles: {6}",
                        GC.GetTotalMemory(false),
                        entity_manager.Entities.Length,
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
