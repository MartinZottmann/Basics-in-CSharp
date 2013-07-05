using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MartinZottmann.Game.IO
{
    public class FileSystem
    {
        public IFormatter Formatter = new BinaryFormatter();

        public void Save(string filepath, object @object)
        {
            using (var stream = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.None))
                Formatter.Serialize(stream, @object);
        }

        public T Load<T>(string filepath)
        {
            using (var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
                return Load<T>(stream);
        }

        public T Load<T>(Stream stream)
        {
            return (T)Formatter.Deserialize(stream);
        }
    }
}
