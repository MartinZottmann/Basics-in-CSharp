using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.States;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MartinZottmann.Engine.UnitTests
{
    [TestClass]
    public class States
    {
        public class Component1 : IComponent { }

        public class Component2 : IComponent { }

        public class Component3 : IComponent { }

        public class Component4 : IComponent { }

        [TestMethod]
        public void StateMachineType()
        {
            // Arrange
            var entity = new Entity();
            var component1_at_arrange = new Component1();
            entity.Add(component1_at_arrange);
            var machine = new StateMachine<Entity, IComponent>(entity, (IComponent s) => entity.Add(s), (IComponent s) => entity.Remove(s));
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
        public void StateMachineInstance()
        {
            // Arrange
            var entity = new Entity();
            var machine = new StateMachine<Entity, IComponent>(entity, (IComponent s) => entity.Add(s), (IComponent s) => entity.Remove(s));
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
        public void DynamicStateMachine()
        {
            // Arrange
            var dictionary = new Dictionary<Type, IComponent>();
            var machine = new StateMachine<Dictionary<Type, IComponent>, IComponent>(
                dictionary,
                (IComponent s) => dictionary.Add(s.GetType(), s),
                (IComponent s) => dictionary.Remove(s.GetType())
            );
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
    }
}
