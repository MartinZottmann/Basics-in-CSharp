using MartinZottmann.Engine.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MartinZottmann.Engine.UnitTests
{
    [TestClass]
    public class Entities_EntityManager
    {
        public class ComponentA : IComponent { }

        public class ComponentB : IComponent { }

        public class ComponentC : IComponent { }

        public class NodeAB : Node
        {
            public ComponentA A;

            public ComponentB B;
        }

        public class NodeBC : Node
        {
            public ComponentB B;

            public ComponentC C;
        }

        public class NodeAbC : Node
        {
            public ComponentA A;

            [OptionalComponent]
            public ComponentB B;

            public ComponentC C;
        }

        [TestMethod]
        public void EntityManager_General()
        {
            // Arrange
            var m = new EntityManager();
            var e1 = new Entity();
            var e2 = new Entity();
            var e3 = new Entity();
            var e4 = new Entity();
            var component_a = new ComponentA();
            var component_b = new ComponentB();
            var component_c = new ComponentC();

            // Act #1
            var node_ab = m.GetNodeList<NodeAB>();
            var node_bc = m.GetNodeList<NodeBC>();
            var node_abc = m.GetNodeList<NodeAbC>();
            e1.Add(component_a);
            e1.Add(component_b);
            m.AddEntity(e1);
            e2.Add(component_b);
            e2.Add(component_c);
            m.AddEntity(e2);
            e3.Add(component_a);
            e3.Add(component_c);
            m.AddEntity(e3);
            e4.Add(component_a);
            e4.Add(component_b);
            e4.Add(component_c);
            m.AddEntity(e4);

            // Assert #
            Assert.AreEqual(node_ab.Nodes.Length, 2);
            Assert.AreEqual(node_ab.Nodes[0].Entity, e1);
            Assert.AreEqual(node_ab.Nodes[0].A, component_a);
            Assert.AreEqual(node_ab.Nodes[0].B, component_b);
            Assert.AreEqual(node_ab.Nodes[1].Entity, e4);
            Assert.AreEqual(node_ab.Nodes[1].A, component_a);
            Assert.AreEqual(node_ab.Nodes[1].B, component_b);

            Assert.AreEqual(node_bc.Nodes.Length, 2);
            Assert.AreEqual(node_bc.Nodes[0].Entity, e2);
            Assert.AreEqual(node_bc.Nodes[0].B, component_b);
            Assert.AreEqual(node_bc.Nodes[0].C, component_c);
            Assert.AreEqual(node_bc.Nodes[1].Entity, e4);
            Assert.AreEqual(node_bc.Nodes[1].B, component_b);
            Assert.AreEqual(node_bc.Nodes[1].C, component_c);

            Assert.AreEqual(node_abc.Nodes.Length, 2);
            Assert.AreEqual(node_abc.Nodes[0].Entity, e3);
            Assert.AreEqual(node_abc.Nodes[0].A, component_a);
            Assert.AreEqual(node_abc.Nodes[0].B, null);
            Assert.AreEqual(node_abc.Nodes[0].C, component_c);
            Assert.AreEqual(node_abc.Nodes[1].Entity, e4);
            Assert.AreEqual(node_abc.Nodes[1].A, component_a);
            Assert.AreEqual(node_abc.Nodes[1].B, component_b);
            Assert.AreEqual(node_abc.Nodes[1].C, component_c);

            // Act #2
            e4.Remove(component_b);

            // Assert #2
            Assert.AreEqual(node_ab.Nodes.Length, 1);
            Assert.AreEqual(node_ab.Nodes[0].Entity, e1);
            Assert.AreEqual(node_ab.Nodes[0].A, component_a);
            Assert.AreEqual(node_ab.Nodes[0].B, component_b);

            Assert.AreEqual(node_bc.Nodes.Length, 1);
            Assert.AreEqual(node_bc.Nodes[0].Entity, e2);
            Assert.AreEqual(node_bc.Nodes[0].B, component_b);
            Assert.AreEqual(node_bc.Nodes[0].C, component_c);

            Assert.AreEqual(node_abc.Nodes.Length, 2);
            Assert.AreEqual(node_abc.Nodes[0].Entity, e3);
            Assert.AreEqual(node_abc.Nodes[0].A, component_a);
            Assert.AreEqual(node_abc.Nodes[0].B, null);
            Assert.AreEqual(node_abc.Nodes[0].C, component_c);
            Assert.AreEqual(node_abc.Nodes[1].Entity, e4);
            Assert.AreEqual(node_abc.Nodes[1].A, component_a);
            Assert.AreEqual(node_abc.Nodes[1].B, null);
            Assert.AreEqual(node_abc.Nodes[1].C, component_c);
        }
    }
}
