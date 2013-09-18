using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace MartinZottmann.Engine.Serialization
{
    public class Serializer
    {
        public const string Prefix = null;

        public const string Namespace = null;

        public const string IdAttributeName = "_Id";

        public const string ReferenceIdAttributeName = "_ReferenceId";

        public const string TypeAttributeName = "_Type";

        public const string NullType = "null";

        public const string NullAttributeName = "is_null";

        public const string NullAttributeValue = "true";

        public const BindingFlags FieldBindingFlagsBinary = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        public const BindingFlags FieldBindingFlagsNonBinary = BindingFlags.Instance | BindingFlags.Public;

        public const BindingFlags PropertyBindingFlagsBinary = BindingFlags.Default;

        public const BindingFlags PropertyBindingFlagsNonBinary = BindingFlags.Instance | BindingFlags.Public;

        public void Serialize(Stream stream, object @object)
        {
            var settings = new XmlWriterSettings();
            settings.Indent = true;

            using (var writer = XmlWriter.Create(stream, settings))
                Serialize(writer, @object);
        }

        public void Serialize(XmlWriter writer, object @object)
        {
            var serialization_writer = new SerializationWriter(writer);
            serialization_writer.SerializeDocument(@object);
        }

        public object Deserialize(Stream stream)
        {
            var settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            using (var reader = XmlReader.Create(stream, settings))
                return Deserialize(reader);
        }

        public object Deserialize(XmlReader reader)
        {
            var serialization_reader = new SerializationReader(reader);
            var @object = serialization_reader.DeserializeDocument();

            return @object;
        }

        public bool HasParameterlessConstructor(Type type)
        {
            return type
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .Any(s => 0 == s.GetParameters().Length);
        }

        public IEnumerable<FieldInfo> Fields(Type type, BindingFlags binding_flags)
        {
            return type
                .GetFields(binding_flags)
                .Where(s => null == s.GetCustomAttribute<NonSerializedAttribute>())
                .Where(s => null == s.GetCustomAttribute<XmlIgnoreAttribute>());
        }

        public IEnumerable<PropertyInfo> Properties(Type type, BindingFlags binding_flags)
        {
            return type
                .GetProperties(binding_flags)
                .Where(s => s.CanRead)
                .Where(s => s.CanWrite)
                .Where(s => 0 == s.GetIndexParameters().Length)
                .Where(s => null == s.GetCustomAttribute<NonSerializedAttribute>())
                .Where(s => null == s.GetCustomAttribute<XmlIgnoreAttribute>());
        }
    }
}
