using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MartinZottmann.Game.IO
{
    public class FileSystem
    {
        public string FilePath;

        public IFormatter Formatter = new BinaryFormatter();

        public FileSystem(string filepath)
        {
            FilePath = filepath;
        }

        public void Save(object @object)
        {
            using (var stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                Formatter.Serialize(stream, @object);
        }

        public T Load<T>() where T : class
        {
            if (File.Exists(FilePath))
                using (var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    if (stream.Length != 0)
                        return Load<T>(stream);

            return null;
        }

        public T Load<T>(Stream stream)
        {
            return (T)Formatter.Deserialize(stream);
        }
    }
}
