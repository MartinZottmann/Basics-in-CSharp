using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime;
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
        public void SerializeDeserialize()
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

        public bool EqualsDeep(object a, object b)
        {
            var stack = new Stack();
            stack.Push(a);
            stack.Push(b);
            var result = EqualsDeep(a, b, stack);
            stack.Pop();
            stack.Pop();
            return result;
        }

        protected bool EqualsDeep(object a, object b, Stack stack)
        {
            if (null == a && null == b)
                return true;

            if (null == a || null == b)
                return false;

            var type_a = a.GetType();
            var type_b = b.GetType();

            if (type_a != type_b)
                return false;

            if (type_a.IsEnum || type_a.IsPrimitive || type_a == typeof(String))
                return Object.Equals(a, b);

            var fields = type_a.GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var value_a = field.GetValue(a);
                var value_b = field.GetValue(b);

                if (stack.Contains(value_a) && stack.Contains(value_b))
                    continue;

                stack.Push(value_a);
                stack.Push(value_b);
                var result = EqualsDeep(value_a, value_b, stack);
                stack.Pop();
                stack.Pop();
                if (!result)
                    return false;
            }

            var properties = type_a.GetProperties(BindingFlags.Public | BindingFlags.Instance);

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

                if (stack.Contains(value_a) && stack.Contains(value_b))
                    continue;

                stack.Push(value_a);
                stack.Push(value_b);
                var result = EqualsDeep(value_a, value_b, stack);
                stack.Pop();
                stack.Pop();
                if (!result)
                    return false;
            }

            return true;
        }
    }
}
