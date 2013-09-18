using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace MartinZottmann.Engine.Serialization
{
    public class SerializationReader : Serializer
    {
        public XmlReader Reader { get; protected set; }

        protected Dictionary<int, object> references;

        public SerializationReader(XmlReader reader)
        {
            Reader = reader;
        }

        public object DeserializeDocument()
        {
            ReadDeclaration();
            ReadElement();

            references = new Dictionary<int, object>();
            var @object = DeserializeElement();
            references = null;

            ReadEOF();

            return @object;
        }

        public object DeserializeElement()
        {
            var is_empty_element = Reader.IsEmptyElement;
            var node = new XmlNode()
            {
                IsEmptyElement = is_empty_element,
                Attributes = GetAttributes()
            };

            if (!node.IsEmptyElement)
                if (!Reader.Read())
                    throw new Exception();

            var type_name = node.Attributes[TypeAttributeName];

            object @object;
            if (NullType == type_name || (node.Attributes.ContainsKey(NullAttributeName) && NullAttributeValue == node.Attributes[NullAttributeName]))
            {
                if (!node.IsEmptyElement)
                    throw new Exception();

                @object = null;
            }
            else
            {
                var type = GetType(type_name);
                node.Type = type;

                if (type.IsArray)
                    @object = DeserializeArray(node);
                else if (type.IsEnum)
                    @object = DeserializeEnum(node);
                else if (type.IsPrimitive)
                    @object = DeserializePrimitive(node);
                else if (typeof(String) == type)
                    @object = DeserializeString(node);
                else if (Type.GetType("System.RuntimeType") == type)
                    @object = DeserializeType(node);
                else if (type.IsClass)
                    @object = DeserializeClass(node);
                else if (type.IsValueType)
                    @object = DeserializeValueType(node);
                else
                    throw new Exception();
            }

            return @object;
        }

        public object DeserializeArray(XmlNode node)
        {
            var cast_type = node.Type.GetElementType();
            if (node.IsEmptyElement)
                return Array.CreateInstance(cast_type, 0);

            var list = new ArrayList();
            do
                switch (Reader.NodeType)
                {
                    case XmlNodeType.Element:
                        list.Add(DeserializeElement());
                        break;
                    case XmlNodeType.EndElement:
                        var @object = Array.CreateInstance(cast_type, list.Count);
                        Array.Copy(list.ToArray(), @object, list.Count);
                        return @object;
                    default:
                        throw new Exception();
                }
            while (Reader.Read());
            throw new Exception();
        }

        public object DeserializeEnum(XmlNode node)
        {
            if (node.IsEmptyElement)
                return null;

            if (XmlNodeType.Text != Reader.NodeType)
                return null;

            var @enum = Enum.Parse(node.Type, Reader.Value);
            var @object = Convert.ChangeType(@enum, node.Type);
            ReadEndElement();

            return @object;
        }

        public object DeserializePrimitive(XmlNode node)
        {
            if (node.IsEmptyElement)
                return null;

            if (XmlNodeType.Text != Reader.NodeType)
                return null;

            var @object = ParsePrimitive(node.Type, Reader.Value);
            ReadEndElement();

            return @object;
        }

        public object DeserializeString(XmlNode node)
        {
            if (node.IsEmptyElement)
                return null;

            if (XmlNodeType.CDATA != Reader.NodeType)
                throw new Exception();

            var @object = Reader.Value;
            ReadEndElement();

            return @object;
        }

        public object DeserializeType(XmlNode node)
        {
            if (node.IsEmptyElement)
                return null;

            if (XmlNodeType.Text != Reader.NodeType)
                throw new Exception();

            var @object = GetType(Reader.Value);
            ReadEndElement();

            return @object;
        }

        public object DeserializeClass(XmlNode node)
        {
            if (node.Attributes.ContainsKey(ReferenceIdAttributeName))
                return references[Int32.Parse(node.Attributes[ReferenceIdAttributeName])];

            if (typeof(ISerializable).IsAssignableFrom(node.Type))
                return DeserializeISerializable(node);

            var @object = CreateObject(node.Type);
            references.Add(Int32.Parse(node.Attributes[IdAttributeName]), @object);

            if (!node.IsEmptyElement)
                node.Values = GetValues();
            DeserializeFields(node, @object);
            DeserializeProperties(node, @object);

            return @object;
        }

        public object DeserializeValueType(XmlNode node)
        {
            if (typeof(ISerializable).IsAssignableFrom(node.Type))
                return DeserializeISerializable(node);

            var @object = CreateObject(node.Type);

            if (!node.IsEmptyElement)
                node.Values = GetValues();
            DeserializeFields(node, @object);
            DeserializeProperties(node, @object);

            return @object;
        }

        public object DeserializeISerializable(XmlNode node)
        {
            var @object = CreateObject(node.Type);
            references.Add(Int32.Parse(node.Attributes[IdAttributeName]), @object);

            var constructor = node.Type.GetConstructor(BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance, null, new[] { typeof(SerializationInfo), typeof(StreamingContext) }, null);
            if (!node.IsEmptyElement)
                node.Values = GetValues();
            var info = new SerializationInfo(node.Type, new FormatterConverter());
            foreach (var value in node.Values)
                info.AddValue(value.Key, value.Value);
            constructor.Invoke(@object, new object[] { info, new StreamingContext(StreamingContextStates.All) });
            var on_deserialization = node.Type.GetMethod("OnDeserialization", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(Object) }, null);
            if (null != on_deserialization)
                on_deserialization.Invoke(@object, new[] { this });

            return @object;
        }

        public void DeserializeFields(XmlNode node, object @object)
        {
            var binding_flags = node.Type.IsSerializable ? FieldBindingFlagsBinary : FieldBindingFlagsNonBinary;

            foreach (var field in Fields(node.Type, binding_flags))
            {
                var value = DeserializeMember(node, field, field.FieldType);
                field.SetValue(@object, value);
            }
        }

        public void DeserializeProperties(XmlNode node, object @object)
        {
            var binding_flags = node.Type.IsSerializable ? PropertyBindingFlagsBinary : PropertyBindingFlagsNonBinary;

            foreach (var property in Properties(node.Type, binding_flags))
            {
                var value = DeserializeMember(node, property, property.PropertyType);
                property.SetValue(@object, value);
            }
        }

        public object DeserializeMember(XmlNode node, MemberInfo member_info, Type member_type)
        {
            var xml_attribute_attribute = member_info.GetCustomAttribute<XmlAttributeAttribute>();
            var xml_array_attribute = member_info.GetCustomAttribute<XmlArrayAttribute>();
            var xml_item_attribute = member_info.GetCustomAttribute<XmlArrayItemAttribute>();

            if (null == xml_attribute_attribute)
                return node.Values[member_info.Name];
            else if (node.Attributes.ContainsKey(member_info.Name))
                return ParsePrimitive(member_type, node.Attributes[member_info.Name]);
            else
                throw new Exception();
        }

        public object CreateObject(Type type)
        {
            return type.IsSerializable
                ? FormatterServices.GetUninitializedObject(type)
                : Activator.CreateInstance(type);
        }

        public Dictionary<string, string> GetAttributes()
        {
            var attributes = new Dictionary<string, string>();
            if (Reader.MoveToFirstAttribute())
                do
                    attributes.Add(Reader.Name, Reader.Value);
                while (Reader.MoveToNextAttribute());
            return attributes;
        }

        public Dictionary<string, object> GetValues()
        {
            var values = new Dictionary<string, object>();
            do
                switch (Reader.NodeType)
                {
                    case XmlNodeType.Element:
                        var key = Reader.Name;
                        var @object = DeserializeElement();
                        values.Add(key, @object);
                        break;
                    case XmlNodeType.EndElement:
                        return values;
                    default:
                        throw new Exception();
                }
            while (Reader.Read());
            throw new Exception();
        }

        protected Type GetType(string fullname)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = assembly.GetType(fullname);
                if (null != type)
                    return type;
            }
            throw new Exception();
        }

        protected object ParsePrimitive(Type type, string value)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean: return Boolean.Parse(value);
                case TypeCode.Byte: return Byte.Parse(value);
                case TypeCode.Char: return Char.Parse(value);
                case TypeCode.DateTime: return DateTime.Parse(value);
                case TypeCode.Decimal: return Decimal.Parse(value);
                case TypeCode.Double: return Double.Parse(value);
                case TypeCode.Int16: return Int16.Parse(value);
                case TypeCode.Int32: return Int32.Parse(value);
                case TypeCode.Int64: return Int64.Parse(value);
                case TypeCode.SByte: return SByte.Parse(value);
                case TypeCode.Single: return Single.Parse(value);
                case TypeCode.String: return value;
                case TypeCode.UInt16: return UInt16.Parse(value);
                case TypeCode.UInt32: return UInt32.Parse(value);
                case TypeCode.UInt64: return UInt64.Parse(value);
                default: throw new Exception();
            }
        }

        public void ReadDeclaration()
        {
            if (!Reader.Read())
                throw new Exception();
            if (XmlNodeType.XmlDeclaration != Reader.NodeType)
                throw new Exception();
        }

        public void ReadElement(string expected_name = null)
        {
            if (!Reader.Read())
                throw new Exception();
            if (XmlNodeType.Element != Reader.NodeType)
                throw new Exception();
            if (null != expected_name)
                if (Reader.Name != expected_name)
                    throw new Exception();
        }

        public void ReadEndElement(string expected_name = null)
        {
            if (!Reader.Read())
                throw new Exception();
            if (XmlNodeType.EndElement != Reader.NodeType)
                throw new Exception();
            if (null != expected_name)
                if (Reader.Name != expected_name)
                    throw new Exception();
        }

        public void ReadEOF()
        {
            if (Reader.Read())
                throw new Exception();
            if (!Reader.EOF)
                throw new Exception();
        }
    }
}
