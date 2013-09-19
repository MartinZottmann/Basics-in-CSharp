using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Serialization;
using MartinZottmann.Engine.States;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace MartinZottmann.Engine.UnitTests
{
    [TestClass]
    public class States_StateMachine
    {
        public class Component1 : IComponent { }

        public class Component2 : IComponent { }

        public class Component3 : IComponent { }

        public class Component4 : IComponent { }

        public class StateDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializable, IStatable<StateDictionary<TKey, TValue>, TKey, TValue>
        {
            public StateDictionary() : base() { }

            protected StateDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

            #region IStatable<StateDictionary<TKey,TValue>,TKey,TValue> Members

            void IStatable<StateDictionary<TKey, TValue>, TKey, TValue>.Add(TKey key, TValue value)
            {
                Add(key, value);
            }

            void IStatable<StateDictionary<TKey, TValue>, TKey, TValue>.Remove(TKey key)
            {
                Remove(key);
            }

            #endregion
        }

        [TestMethod]
        public void StateMachine_Type()
        {
            // Arrange
            var entity = new Entity();
            var component1_at_arrange = new Component1();
            entity.Add(component1_at_arrange);
            var machine = new StateMachine<Entity, IComponent>(entity);
            machine
                .CreateState("A")
                .Add(typeof(Component2));
            machine
                .CreateState("B")
                .Add(typeof(Component3))
                .Add<Component4>();
            machine
                .CreateState("C")
                .Add(typeof(Component3));

            // Act & Assert
            Assert.AreEqual(entity.components.Count, 1);
            Assert.AreEqual(entity.Get<Component1>(), component1_at_arrange);

            machine.ChangeState("A");
            Assert.AreEqual(entity.components.Count, 2);
            Assert.AreEqual(entity.Get<Component1>(), component1_at_arrange);
            var component2_at_first = entity.Get<Component2>();
            Assert.AreEqual(component2_at_first.GetType(), typeof(Component2));

            machine.ChangeState("B");
            Assert.AreEqual(entity.components.Count, 3);
            Assert.AreEqual(entity.Get<Component1>(), component1_at_arrange);
            var component3_at_first = entity.Get<Component3>();
            Assert.AreEqual(component3_at_first.GetType(), typeof(Component3));
            Assert.AreEqual(entity.Get<Component4>().GetType(), typeof(Component4));

            machine.ChangeState("C");
            Assert.AreEqual(entity.components.Count, 2);
            Assert.AreEqual(entity.Get<Component1>(), component1_at_arrange);
            var component3_at_second = entity.Get<Component3>();
            Assert.AreEqual(component3_at_second.GetType(), typeof(Component3));
            Assert.AreEqual(component3_at_first, component3_at_second);

            machine.ChangeState("A");
            Assert.AreEqual(entity.components.Count, 2);
            Assert.AreEqual(entity.Get<Component1>(), component1_at_arrange);
            var component2_at_second = entity.Get<Component2>();
            Assert.AreEqual(component2_at_second.GetType(), typeof(Component2));
            Assert.AreNotEqual(component2_at_first, component2_at_second);
        }

        [TestMethod]
        public void StateMachine_Instance()
        {
            // Arrange
            var entity = new Entity();
            var machine = new StateMachine<Entity, IComponent>(entity);
            var component2_at_arrange = new Component2() { };
            machine.CreateState("A").Add(component2_at_arrange);
            machine.CreateState("B").Add(new Component3() { });

            // Act & Assert
            Assert.AreEqual(entity.components.Count, 0);

            machine.ChangeState("A");
            Assert.AreEqual(entity.components.Count, 1);
            var component2_at_first = entity.Get<Component2>();
            Assert.AreEqual(component2_at_first.GetType(), typeof(Component2));
            Assert.AreEqual(component2_at_first, component2_at_arrange);

            machine.ChangeState("B");
            Assert.AreEqual(entity.components.Count, 1);
            Assert.AreEqual(entity.Get<Component3>().GetType(), typeof(Component3));

            machine.ChangeState("A");
            Assert.AreEqual(entity.components.Count, 1);
            var component2_at_second = entity.Get<Component2>();
            Assert.AreEqual(component2_at_second.GetType(), typeof(Component2));
            Assert.AreEqual(component2_at_second, component2_at_first);
        }

        [TestMethod]
        public void StateMachine_SerializeDeserialize()
        {
            // Arrange
            var encoding = Encoding.UTF8;
            var serializer = new Serializer();
            var target = new StateDictionary<string, string>();
            var subject = new StateMachine<StateDictionary<string, string>, string, string>(target);
            subject.CreateState("Hex2Dec")
                .Add("A", "10")
                .Add("B", "11");
            subject.CreateState("Dec2Hex")
                .Add("10", "A")
                .Add("11", "B");
            subject.ChangeState("Hex2Dec");
            var @object = new Serialization_TestObject(subject);

            // Act #1
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, @object.Object);
                @object.Serialized = encoding.GetString(stream.ToArray());
            }

            // Act #2
            var buffer = encoding.GetBytes(@object.Serialized);
            using (var stream = new MemoryStream(buffer))
                @object.Deserialized = serializer.Deserialize(stream);

            // Assert
            Assert.IsTrue(new Helper().EqualsDeep(subject, @object.Object));
        }
    }
}
