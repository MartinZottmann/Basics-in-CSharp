using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace MartinZottmann.Engine.Serialization
{
    public class SerializationWriter : Serializer
    {
        public XmlWriter Writer { get; protected set; }

        protected ObjectReference references = new ObjectReference();

        public SerializationWriter(XmlWriter writer)
        {
            Writer = writer;
        }

        public void SerializeDocument(object @object)
        {
            if (0 != references.Count)
                throw new Exception("Cannot use SerializeRoot inside of SerializeRoot.");

            Writer.WriteStartDocument();
            SerializeObject(@object);
            Writer.WriteEndDocument();

            references.Clear();
        }

        public void SerializeEntry(SerializationEntry entry)
        {
            SerializeObject(entry.Value, entry.ObjectType, entry.Name);
        }

        public void SerializeObject(object @object, Type type = null, string name = null, string child_name = null)
        {
            if (null != @object && null == type)
                type = @object.GetType();

            WriteStartElement(type, name);

            if (null == @object)
                SerializeNull();
            else if (type.IsArray)
                SerializeArray(@object, child_name);
            else if (type.IsEnum)
                SerializeEnum(@object);
            else if (type.IsPrimitive)
                SerializePrimitive(@object);
            else if (typeof(String) == type)
                SerializeString((String)@object);
            else if (Type.GetType("System.RuntimeType") == type)
                SerializeType((Type)@object);
            else if (type.IsClass)
                SerializeClass(@object);
            else if (type.IsValueType)
                SerializeValueType(@object);
            else
                throw new Exception();

            WriteEndElement();
        }

        public void SerializeNull()
        {
            Writer.WriteAttributeString(NullAttributeName, NullAttributeValue);
        }

        public void SerializeArray(object @object, string element_name = null)
        {
            var type = @object.GetType();

            foreach (var i in (Array)@object)
                SerializeObject(i, null == i ? type.GetElementType() : i.GetType(), element_name);
        }

        public void SerializeEnum(object @object)
        {
            Writer.WriteString(@object.ToString());
        }

        public void SerializePrimitive(object @object)
        {
            Writer.WriteString(@object.ToString());
        }

        public void SerializeString(string @object)
        {
            Writer.WriteCData(@object);
        }

        public void SerializeType(Type @object)
        {
            Writer.WriteString(@object.FullName);
        }

        public void SerializeClass(object @object)
        {
            var type = @object.GetType();
            var context = new StreamingContext(StreamingContextStates.All);

            if (references.Contains(@object))
            {
                CallOnSerializingMethods(@object, context);
                WriteReferenceIDAttribute(@object);
                CallOnSerializedMethods(@object, context);
                return;
            }

            WriteIDAttribute(@object);

            if (@object is ISerializable)
            {
                SerializerISerializable(@object, context);
                return;
            }

            CallOnSerializingMethods(@object, context);
            WriteMembers(@object);
            CallOnSerializedMethods(@object, context);
        }

        public void SerializeValueType(object @object)
        {
            var type = @object.GetType();
            var context = new StreamingContext(StreamingContextStates.All);

            if (@object is ISerializable)
            {
                SerializerISerializable(@object, context);
                return;
            }

            CallOnSerializingMethods(@object, context);
            WriteMembers(@object);
            CallOnSerializedMethods(@object, context);
        }

        public void SerializerISerializable(object @object, StreamingContext context)
        {
            var type = @object.GetType();
            var info = new SerializationInfo(type, new FormatterConverter());

            CallOnSerializingMethods(@object, context);
            ((ISerializable)@object).GetObjectData(info, new StreamingContext(StreamingContextStates.All));
            foreach (var entry in info)
                SerializeObject(entry.Value, null, entry.Name);
            CallOnSerializedMethods(@object, context);
        }

        protected void CallOnSerializingMethods(object @object, StreamingContext context)
        {
            @object
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(s => null != s.GetCustomAttribute<OnSerializingAttribute>())
                .Select(s => s.Invoke(@object, new object[] { context }));
        }

        protected void CallOnSerializedMethods(object @object, StreamingContext context)
        {
            @object
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(s => null != s.GetCustomAttribute<OnSerializedAttribute>())
                .Select(s => s.Invoke(@object, new object[] { context }));
        }

        public void WriteMembers(object @object)
        {
            var type = @object.GetType();
            var field_binding_flags = type.IsSerializable ? FieldBindingFlagsBinary : FieldBindingFlagsNonBinary;
            var property_binding_flags = type.IsSerializable ? PropertyBindingFlagsBinary : PropertyBindingFlagsNonBinary;
            var members = (
                    Fields(type, field_binding_flags)
                        .Select(s => new Member() { MemberInfo = s, MemberType = s.FieldType, Value = s.GetValue(@object) })
                ).Concat(
                    Properties(type, property_binding_flags)
                        .Select(s => new Member() { MemberInfo = s, MemberType = s.PropertyType, Value = s.GetValue(@object) })
                )
                .OrderBy(s => null == s.MemberInfo.GetCustomAttribute<XmlAttributeAttribute>());

            foreach (var member in members)
                SerializeMember(member.MemberInfo, member.MemberType, member.Value);
        }

        public void SerializeMember(MemberInfo member_info, Type member_type, object value)
        {
            var xml_attribute_attribute = member_info.GetCustomAttribute<XmlAttributeAttribute>();
            var xml_array_attribute = member_info.GetCustomAttribute<XmlArrayAttribute>();
            var xml_array_item_attribute = member_info.GetCustomAttribute<XmlArrayItemAttribute>();

            var type = null == value ? member_type : value.GetType();
            var name = member_info.Name;
            string child_name = null;

            if (null == xml_attribute_attribute)
            {
                if (member_type.IsArray)
                {
                    if (null != xml_array_attribute)
                        name = xml_array_attribute.ElementName;
                    if (null != xml_array_item_attribute)
                        child_name = xml_array_item_attribute.ElementName;
                }

                SerializeObject(value, type, name, child_name);
            }
            else
            {
                if (member_type.IsArray)
                    throw new Exception("Cannot use XmlAttributeAttribute on an array object.");

                Writer.WriteAttributeString(name, value.ToString());
            }
        }

        public void WriteStartElement(Type type, string name)
        {
            Writer.WriteStartElement(name ?? "Object");
            Writer.WriteAttributeString(Prefix, TypeAttributeName, Namespace, null == type ? NullType : type.FullName);
        }

        public void WriteIDAttribute(object @object)
        {
            references.Add(@object);
            Writer.WriteAttributeString(Prefix, IdAttributeName, Namespace, references.GetIDValue(@object));
        }

        public void WriteReferenceIDAttribute(object @object)
        {
            Writer.WriteAttributeString(Prefix, ReferenceIdAttributeName, Namespace, references.GetIDValue(@object));
        }

        public void WriteEndElement()
        {
            Writer.WriteEndElement();
        }

        protected class Member
        {
            public MemberInfo MemberInfo;

            public Type MemberType;

            public object Value;
        }
    }
}
