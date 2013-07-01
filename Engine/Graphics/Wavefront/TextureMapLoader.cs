using System.IO;

namespace MartinZottmann.Engine.Graphics.Wavefront
{
    public class TextureMapLoader
    {
        public TextureMapFile Load(string filepath)
        {
            var file_info = new FileInfo(filepath);
            return Load(file_info.DirectoryName, file_info.Name);
        }

        public TextureMapFile Load(string directory, string filename_or_filepath)
        {
            var texture_map_loader = new TextureMapLoader();
            var filepath = File.Exists(filename_or_filepath) ? filename_or_filepath : directory + Path.DirectorySeparatorChar + filename_or_filepath;
            return new TextureMapFile(filepath);
        }
    }
}
