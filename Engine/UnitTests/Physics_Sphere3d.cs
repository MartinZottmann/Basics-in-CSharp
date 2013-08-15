using MartinZottmann.Engine.Physics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;

namespace MartinZottmann.Engine.UnitTests
{
    [TestClass]
    public class Physics_Sphere3d
    {

        [TestMethod]
        public void Collides_Sphere3d()
        {
            // Arrange
            var s0 = new Sphere3d(new Vector3d(-2, 0, 0), 3);
            var s1 = new Sphere3d(new Vector3d(2, 0, 0), 3);

            // Act
            var c0 = s0.Collides(s1);
            var c1 = s1.Collides(s0);

            // Assert
            Assert.AreEqual(new Vector3d(1, 0, 0), c0.HitPoint);
            Assert.AreEqual(new Vector3d(-2, 0, 0), c0.Normal);
            Assert.AreEqual(s0, c0.Object0);
            Assert.AreEqual(s1, c0.Object1);
            Assert.AreEqual(null, c0.Parent);
            Assert.AreEqual(2, c0.PenetrationDepth);
            Assert.AreEqual(new Vector3d(-1, 0, 0), c1.HitPoint);
            Assert.AreEqual(new Vector3d(2, 0, 0), c1.Normal);
            Assert.AreEqual(s1, c1.Object0);
            Assert.AreEqual(s0, c1.Object1);
            Assert.AreEqual(null, c1.Parent);
            Assert.AreEqual(2, c1.PenetrationDepth);
        }
    }
}
