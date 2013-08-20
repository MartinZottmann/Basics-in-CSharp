using MartinZottmann.Engine.Graphics.OpenGL;

namespace MartinZottmann.Engine.Resources
{
    public class Textures : Resource<Texture>
    {
        public Textures(ResourceManager resource_manager) : base(resource_manager) { }

        //public override void LoadFromFile(string filename)
        //{
        //    Load(filename);
        //}

        public Texture Load(string filename, bool mipmapped = false, OpenTK.Graphics.OpenGL.TextureTarget target = OpenTK.Graphics.OpenGL.TextureTarget.Texture2D)
        {
            if (!resources.ContainsKey(filename))
                this[filename] = new Texture(filename, mipmapped, target);

            return this[filename];
        }
    }
}
