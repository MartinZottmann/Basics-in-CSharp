using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.States;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MartinZottmann.Engine.UnitTests
{
    [TestClass]
    public class States_DynamicStateMachine
    {
        public class Component1 : IComponent { }

        public class Component2 : IComponent { }

        public class Component3 : IComponent { }

        public class Component4 : IComponent { }

        [TestMethod]
        public void DynamicStateMachine_Type()
        {
            // Arrange
            var entity = new Entity();
            var component1_at_arrange = new Component1();
            entity.Add(component1_at_arrange);
            var machine = new DynamicStateMachine<Entity, IComponent>(entity, s => entity.Add(s), s => entity.Remove(s));
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
        public void DynamicStateMachine_Instance()
        {
            // Arrange
            var entity = new Entity();
            var machine = new DynamicStateMachine<Entity, IComponent>(entity, s => entity.Add(s), s => entity.Remove(s));
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
        public void DynamicStateMachine_Key()
        {
            // Arrange
            var dictionary = new Dictionary<Type, IComponent>();
            var machine = new DynamicStateMachine<Dictionary<Type, IComponent>, IComponent>(dictionary, s => dictionary.Add(s.GetType(), s), s => dictionary.Remove(s.GetType()));
            machine.CreateState("A").Add(typeof(Component2));
            machine.CreateState("B").Add(typeof(Component3));

            // Act & Assert
            Assert.AreEqual(dictionary.Count, 0);

            machine.ChangeState("A");
            Assert.AreEqual(dictionary.Count, 1);
            Assert.AreEqual(dictionary[typeof(Component2)].GetType(), typeof(Component2));

            machine.ChangeState("B");
            Assert.AreEqual(dictionary.Count, 1);
            Assert.AreEqual(dictionary[typeof(Component3)].GetType(), typeof(Component3));

            machine.ChangeState("A");
            Assert.AreEqual(dictionary.Count, 1);
            Assert.AreEqual(dictionary[typeof(Component2)].GetType(), typeof(Component2));
        }

        [TestMethod]
        public void DynamicStateMachine_KeyValue()
        {
            // Arrange
            var dictionary = new Dictionary<string, string>();
            var machine = new DynamicStateMachine<Dictionary<string, string>, string, string>(dictionary, (k, v) => dictionary.Add(k, v), k => dictionary.Remove(k));
            machine.CreateState("Hex2Dec")
                .Add("A", "10")
                .Add("B", "11");
            machine.CreateState("Dec2Hex")
                .Add("10", "A")
                .Add("11", "B");
            machine.CreateState("Dec2Bin")
                .Add("10", "1010")
                .Add("11", "1011");

            // Act & Assert
            Assert.AreEqual(dictionary.Count, 0);

            machine.ChangeState("Hex2Dec");
            Assert.AreEqual(dictionary.Count, 2);
            Assert.AreEqual(dictionary["A"], "10");
            Assert.AreEqual(dictionary["B"], "11");

            machine.ChangeState("Dec2Hex");
            Assert.AreEqual(dictionary.Count, 2);
            Assert.AreEqual(dictionary["10"], "A");
            Assert.AreEqual(dictionary["11"], "B");

            machine.ChangeState("Dec2Bin");
            Assert.AreEqual(dictionary.Count, 2);
            Assert.AreEqual(dictionary["10"], "1010");
            Assert.AreEqual(dictionary["11"], "1011");
        }
    }
}
