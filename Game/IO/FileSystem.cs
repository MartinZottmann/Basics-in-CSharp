using MartinZottmann.Engine.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace MartinZottmann.Game.IO
{
    public class FileSystem
    {
        public Serializer serializer = new Serializer();

        public string FilePath;

        public FileSystem(string filepath)
        {
            FilePath = filepath;
        }

        public void Save(object @object)
        {
            using (var stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                serializer.Serialize(stream, @object);
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
            var @object = (T)serializer.Deserialize(stream);

            return @object;
        }
    }
}
