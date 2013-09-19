using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Serialization;
using MartinZottmann.Engine.States;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime;
using System.Runtime.Serialization;
using System.Text;

namespace MartinZottmann.Engine.UnitTests
{
    [TestClass]
    public class Serialization_Serializer
    {
        protected class TestObject
        {
            public object Object;

            public string Serialized;

            public object Deserialized;

            public TestObject(object @object)
            {
                Object = @object;
                Serialized = null;
                Deserialized = null;
            }
        }

        [Serializable]
        protected class TestSubject
        {
            public int Id;

            public TestSubject Parent;
        }

        [TestMethod]
        public void SerializeDeserialize_Common()
        {
            // Arrange
            var encoding = Encoding.UTF8;
            var serializer = new Serializer();

            var test_subject_a = new TestSubject() { Id = 1, Parent = null };
            var test_subject_b = new TestSubject() { Id = 2, Parent = test_subject_a };
            var test_subject_c = new TestSubject() { Id = 3 };
            test_subject_c.Parent = test_subject_c;
            var test_subject_d = new TestSubject() { Id = 41 };
            var test_subject_e = new TestSubject() { Id = 42 };
            test_subject_d.Parent = test_subject_e;
            test_subject_e.Parent = test_subject_d;

            var objects = new TestObject[] {
                new TestObject(null),
                new TestObject(new int?(123)),
                new TestObject(new int?()),
                new TestObject(new double?(123.456)),
                new TestObject(new double?()),
                new TestObject(typeof(Int32)),
                new TestObject(typeof(Type)),
                new TestObject(Int16.MinValue),
                new TestObject(Int16.MaxValue),
                new TestObject(Int32.MinValue),
                new TestObject(Int32.MaxValue),
                new TestObject(Int64.MinValue),
                new TestObject(Int64.MaxValue),
                new TestObject(String.Empty),
                new TestObject("Test String"),
                new TestObject(new string[] { null, String.Empty, "X", "XY", "X\tZ", "X\nZ" }),
                new TestObject(Double.Epsilon),
                //new TestObject(Double.MaxValue),
                //new TestObject(Double.MinValue),
                new TestObject(Double.NaN),
                new TestObject(Double.NegativeInfinity),
                new TestObject(Double.PositiveInfinity),
                new TestObject(GCLatencyMode.Batch),
                new TestObject(new Vector3d(123.456, 987, 654)),
                new TestObject(Guid.NewGuid()),
                new TestObject(new int[] {0, 1, 30, 1002}),
                new TestObject(new object[] {0, 1, 30.0, "ABC", null, -1, '*'}),
                new TestObject(new Entity()),
                new TestObject(test_subject_a),
                new TestObject(test_subject_b),
                new TestObject(test_subject_c),
                new TestObject(test_subject_d),
                new TestObject(test_subject_e),
                new TestObject(Tuple.Create(9)),
                new TestObject(Tuple.Create(9, 8)),
                new TestObject(Tuple.Create(9, 8, 7)),
                new TestObject(Tuple.Create(9, 8, 7, 6)),
                new TestObject(Tuple.Create(9, 8, 7, 6, 5)),
                new TestObject(Tuple.Create(9, 8, 7, 6, 5, 4)),
                new TestObject(Tuple.Create(9, 8, 7, 6, 5, 4, 3)),
                new TestObject(Tuple.Create(9, 8, 7, 6, 5, 4, 3, 2)),
                new TestObject(Tuple.Create(9, 8, 7, 6, 5, 4, 3, Tuple.Create(2))),
                new TestObject(Tuple.Create(9, 8, 7, 6, 5, 4, 3, Tuple.Create(2, 1))),
                new TestObject(Tuple.Create(9, 8, 7, 6, 5, 4, 3, Tuple.Create(2, 1, 0))),
                new TestObject(Tuple.Create(9, 8.0, 7, "ABC", 5, 4, 3, Tuple.Create(2, '*', 0))),
                new TestObject(Tuple.Create<object>(9)),
                new TestObject(new List<int>(new[] {9, 8, 7, 6, 5, 4, 3, 2, 1, 0})),
                new TestObject(new List<object>(new object[] {0, 1, 30.0, "ABC", null, -1, '*'})),
                new TestObject(new Dictionary<int, int>() { { 1, 100 }, { 100, 1 } }),
                new TestObject(new Dictionary<object, object>() { { 1, "ABC" }, { '*', 2.5 } })
            };
            var n = objects.Length;

            // Act #1
            for (var i = 0; i < n; i++)
            {
                var @object = objects[i];
                using (var stream = new MemoryStream())
                {
                    serializer.Serialize(stream, @object.Object);
                    @object.Serialized = encoding.GetString(stream.ToArray());
                }
            }

            // Act #2
            for (var i = 0; i < n; i++)
            {
                var @object = objects[i];
                var buffer = encoding.GetBytes(@object.Serialized);
                using (var stream = new MemoryStream(buffer))
                    @object.Deserialized = serializer.Deserialize(stream);
            }

            // Assert
            for (var i = 0; i < n; i++)
            {
                var @object = objects[i];

                if (null != @object.Object)
                {
                    Assert.AreEqual(@object.Object.ToString(), @object.Deserialized.ToString(), "{0} != {1} ({2})", @object.Object.ToString(), @object.Deserialized.ToString(), @object.Serialized);
                    Assert.AreEqual(@object.Object.GetType(), @object.Deserialized.GetType(), "{0} != {1} ({2})", @object.Object.GetType(), @object.Deserialized.GetType(), @object.Serialized);
                }

                if (@object.Object is ICollection)
                {
                    Assert.IsTrue(@object.Deserialized is ICollection);
                    CollectionAssert.AreEqual((ICollection)@object.Object, (ICollection)@object.Deserialized, "{0} != {1} ({2})", @object.Object, @object.Deserialized, @object.Serialized);
                }
                else
                    Assert.IsTrue(EqualsDeep(@object.Object, @object.Deserialized), "{0} != {1} ({2})", @object.Object, @object.Deserialized, @object.Serialized);
                //Assert.AreEqual(@object.Object, @object.Deserialized, "{0} != {1} ({2})", @object.Object, @object.Deserialized, @object.Serialized);
            }
        }

        protected class StateDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializable, IStatable<StateDictionary<TKey, TValue>, TKey, TValue>
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
        public void SerializeDeserialize_StateMachine()
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
            var @object = new TestObject(subject);

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
            Assert.IsTrue(EqualsDeep(subject, @object.Object));
        }

        public bool EqualsDeep(object a, object b)
        {
            var terminators = new HashSet<object>();
            terminators.Add(a);
            terminators.Add(b);
            var result = EqualsDeep(a, b, terminators);
            terminators.Remove(b);
            terminators.Remove(a);
            return result;
        }

        protected bool EqualsDeep(object a, object b, HashSet<object> terminators)
        {
            if (null == a && null == b)
                return true;

            if (null == a || null == b)
                return false;

            if (a is IntPtr || b is IntPtr || a is UIntPtr || b is UIntPtr)
                return false;

            var type_a = a.GetType();
            var type_b = b.GetType();

            if (type_a != type_b)
                return false;

            if (type_a.IsPointer || type_b.IsPointer)
                return false;

            if (type_a.IsEnum || type_a.IsPrimitive || typeof(String) == type_a)
                return Object.Equals(a, b);

            var fields = type_a.GetFields(BindingFlags.Instance | BindingFlags.Public);

            foreach (var field in fields)
            {
                var value_a = field.GetValue(a);
                var value_b = field.GetValue(b);

                if (terminators.Contains(value_a) && terminators.Contains(value_b))
                    continue;

                terminators.Add(value_a);
                terminators.Add(value_b);
                var result = EqualsDeep(value_a, value_b, terminators);
                terminators.Remove(value_b);
                terminators.Remove(value_a);
                if (!result)
                    return false;
            }

            var properties = type_a.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                if (0 != property.GetIndexParameters().Length)
                    continue;

                if (!property.CanRead)
                    continue;

                if (!property.CanWrite)
                    continue;

                var value_a = property.GetValue(a);
                var value_b = property.GetValue(b);

                if (terminators.Contains(value_a) && terminators.Contains(value_b))
                    continue;

                terminators.Add(value_a);
                terminators.Add(value_b);
                var result = EqualsDeep(value_a, value_b, terminators);
                terminators.Remove(value_b);
                terminators.Remove(value_a);
                if (!result)
                    return false;
            }

            return true;
        }
    }
}
