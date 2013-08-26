using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MartinZottmann.Game.IO
{
    public class Serializer
    {
        public void Serialize(Stream stream, object @object)
        {
            Serialize(new StreamWriter(stream), @object);
        }

        public void Serialize(StreamWriter stream, object @object)
        {
            var type = @object.GetType();

            if (type.IsArray)
            {
                stream.WriteLine("array {");
                foreach (var i in (Array)@object)
                    Serialize(stream, i);
                stream.WriteLine("}");
            }
            else
            {
                stream.WriteLine("type");
                stream.WriteLine(type.FullName);
            }

            stream.WriteLine("fields {");
            foreach (var fields in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                stream.WriteLine("field");
                stream.WriteLine(fields.Name);
                Serialize(stream, fields.GetValue(@object));
            }
            stream.WriteLine("}");
        }

        public object Deserialize(Stream stream)
        {
            return Deserialize(new StreamReader(stream));
        }

        public object Deserialize(StreamReader stream)
        {
            return Deserialize(stream, stream.ReadLine());
        }

        public object Deserialize(StreamReader stream, string @string)
        {
            switch (@string)
            {
                case "type":
                    var type_name = stream.ReadLine();
                    var type = Type.GetType(type_name, true);
                    var @object = Activator.CreateInstance(type);
                    return Deserialize(stream, @object);
                case "array {":
                    var @array = new List<object>();
                    while (true)
                    {
                        var line = stream.ReadLine();
                        if (line == "}")
                            break;

                        @array.Add(Deserialize(stream, line));
                    }
                    return @array.ToArray();
                default:
                    throw new Exception(@string);
            }
        }

        public object Deserialize(StreamReader stream, object @object)
        {
            var key = stream.ReadLine();
            switch (key)
            {
                default:
                    throw new Exception(key);
            }

            return @object;
        }
    }
}
