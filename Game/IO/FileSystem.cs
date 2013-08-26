using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace MartinZottmann.Game.IO
{
    public class FileSystem
    {
        public string FilePath;

        public FileSystem(string filepath)
        {
            FilePath = filepath;
        }

        public void Save(object @object)
        {
            using (var stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var serializer = new XmlSerializer(@object.GetType(), GetKnownTypes());
                serializer.Serialize(stream, @object);
            }
        }

        protected Type[] GetKnownTypes()
        {
            var types = new List<Type>();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsSerializable)
                    continue;

                var parameterless_constructor = false;
                foreach (var constructor in type.GetConstructors())
                    if (constructor.GetParameters().Length == 0)
                    {
                        parameterless_constructor = true;
                        break;
                    }
                if (!parameterless_constructor)
                    continue;

                types.Add(type);
            }

            return types.ToArray();
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
            var serializer = new XmlSerializer(typeof(T), GetKnownTypes());
            var @object = (T)serializer.Deserialize(stream);

            return @object;
        }
    }
}
