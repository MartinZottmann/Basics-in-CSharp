namespace MartinZottmann.Engine.UnitTests
{
    public class Serialization_TestObject
    {
        public object Object;

        public string Serialized;

        public object Deserialized;

        public Serialization_TestObject(object @object)
        {
            Object = @object;
            Serialized = null;
            Deserialized = null;
        }
    }
}
