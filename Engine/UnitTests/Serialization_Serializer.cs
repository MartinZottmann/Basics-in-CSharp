using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime;
using System.Text;

namespace MartinZottmann.Engine.UnitTests
{
    [TestClass]
    public class Serialization_Serializer
    {
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

            var objects = new Serialization_TestObject[] {
                new Serialization_TestObject(null),
                new Serialization_TestObject(new int?(123)),
                new Serialization_TestObject(new int?()),
                new Serialization_TestObject(new double?(123.456)),
                new Serialization_TestObject(new double?()),
                new Serialization_TestObject(typeof(Int32)),
                new Serialization_TestObject(typeof(Type)),
                new Serialization_TestObject(Int16.MinValue),
                new Serialization_TestObject(Int16.MaxValue),
                new Serialization_TestObject(Int32.MinValue),
                new Serialization_TestObject(Int32.MaxValue),
                new Serialization_TestObject(Int64.MinValue),
                new Serialization_TestObject(Int64.MaxValue),
                new Serialization_TestObject(String.Empty),
                new Serialization_TestObject("Test String"),
                new Serialization_TestObject(new string[] { null, String.Empty, "X", "XY", "X\tZ", "X\nZ" }),
                new Serialization_TestObject(Double.Epsilon),
                //new Serialization_TestObject(Double.MaxValue),
                //new Serialization_TestObject(Double.MinValue),
                new Serialization_TestObject(Double.NaN),
                new Serialization_TestObject(Double.NegativeInfinity),
                new Serialization_TestObject(Double.PositiveInfinity),
                new Serialization_TestObject(GCLatencyMode.Batch),
                new Serialization_TestObject(new Vector3d(123.456, 987, 654)),
                new Serialization_TestObject(Guid.NewGuid()),
                new Serialization_TestObject(new int[] {0, 1, 30, 1002}),
                new Serialization_TestObject(new object[] {0, 1, 30.0, "ABC", null, -1, '*'}),
                new Serialization_TestObject(new Entity()),
                new Serialization_TestObject(test_subject_a),
                new Serialization_TestObject(test_subject_b),
                new Serialization_TestObject(test_subject_c),
                new Serialization_TestObject(test_subject_d),
                new Serialization_TestObject(test_subject_e),
                new Serialization_TestObject(Tuple.Create(9)),
                new Serialization_TestObject(Tuple.Create(9, 8)),
                new Serialization_TestObject(Tuple.Create(9, 8, 7)),
                new Serialization_TestObject(Tuple.Create(9, 8, 7, 6)),
                new Serialization_TestObject(Tuple.Create(9, 8, 7, 6, 5)),
                new Serialization_TestObject(Tuple.Create(9, 8, 7, 6, 5, 4)),
                new Serialization_TestObject(Tuple.Create(9, 8, 7, 6, 5, 4, 3)),
                new Serialization_TestObject(Tuple.Create(9, 8, 7, 6, 5, 4, 3, 2)),
                new Serialization_TestObject(Tuple.Create(9, 8, 7, 6, 5, 4, 3, Tuple.Create(2))),
                new Serialization_TestObject(Tuple.Create(9, 8, 7, 6, 5, 4, 3, Tuple.Create(2, 1))),
                new Serialization_TestObject(Tuple.Create(9, 8, 7, 6, 5, 4, 3, Tuple.Create(2, 1, 0))),
                new Serialization_TestObject(Tuple.Create(9, 8.0, 7, "ABC", 5, 4, 3, Tuple.Create(2, '*', 0))),
                new Serialization_TestObject(Tuple.Create<object>(9)),
                new Serialization_TestObject(new List<int>(new[] {9, 8, 7, 6, 5, 4, 3, 2, 1, 0})),
                new Serialization_TestObject(new List<object>(new object[] {0, 1, 30.0, "ABC", null, -1, '*'})),
                new Serialization_TestObject(new Dictionary<int, int>() { { 1, 100 }, { 100, 1 } }),
                new Serialization_TestObject(new Dictionary<object, object>() { { 1, "ABC" }, { '*', 2.5 } })
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
                    Assert.IsTrue(new Helper().EqualsDeep(@object.Object, @object.Deserialized), "{0} != {1} ({2})", @object.Object, @object.Deserialized, @object.Serialized);
                //Assert.AreEqual(@object.Object, @object.Deserialized, "{0} != {1} ({2})", @object.Object, @object.Deserialized, @object.Serialized);
            }
        }
    }
}
