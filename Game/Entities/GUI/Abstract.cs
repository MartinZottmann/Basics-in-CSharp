using MartinZottmann.Engine.Graphics.Mesh;
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

        protected FontMeshBuilder font_mesh_builder;

        protected ResourceManager resource_manager;

        public Model Model { get; set; }

        public virtual void Bind(ResourceManager resource_manager, FontMeshBuilder font_mesh_builder)
        {
            this.resource_manager = resource_manager;
            this.font_mesh_builder = font_mesh_builder;

            InitModel();
        }

        public virtual void Update(double delta_time) { }

        public virtual void Render(double delta_time)
        {
            Model.Program.UniformLocations["in_Texture"].Set(0);
            Model.Draw();
        }

        protected void InitModel()
        {
            Model = new Model();
            Model.Program = resource_manager.Programs["plain_texture"];
            Model.Texture = resource_manager.Textures["GUI_font"];
        }
    }
}
