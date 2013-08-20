﻿using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using OpenTK;

namespace MartinZottmann.Game.Entities.GUI
{
    public class Abstract : IGUIElement
    {
        public Vector3d Scale = new Vector3d(1, 1, 1);

        public Quaterniond Orientation = Quaterniond.Identity;

        public Vector3d Position = Vector3d.Zero;

        /// <summary>
        /// Model Matrix = Scale * Rotation * Translation
        /// </summary>
        public Matrix4d ModelMatrix { get { return Matrix4d.Scale(Scale) * Matrix4d.Rotate(Orientation) * Matrix4d.CreateTranslation(Position); } }

        public Texture FontTexture { get; set; }

        public FontMeshBuilder FontMeshBuilder { get; set; }

        public Entity Model { get; set; }

        public virtual void Start(ResourceManager resource_manager)
        {
            Model = new MartinZottmann.Engine.Graphics.OpenGL.Entity();
            Model.Program = resource_manager.Programs["plain_texture"];
            Model.Texture = FontTexture;
        }

        public virtual void Update(double delta_time) { }

        public virtual void Render(double delta_time)
        {
            Model.Draw();
        }
    }
}
