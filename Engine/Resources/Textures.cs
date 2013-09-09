using MartinZottmann.Engine.Graphics.OpenGL;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Engine.Resources
{
    public class Textures : Resource<Texture>
    {
        public Textures(ResourceManager resource_manager) : base(resource_manager) { }

        public Texture Load(string filename, bool mipmapped = false, TextureTarget target = TextureTarget.Texture2D, TextureMinFilter min_filter = TextureMinFilter.Nearest, TextureMagFilter mag_filter = TextureMagFilter.Nearest)
        {
            if (!resources.ContainsKey(filename))
                this[filename] = new Texture(filename, mipmapped, target, min_filter, mag_filter);

            return this[filename];
        }
    }
}
