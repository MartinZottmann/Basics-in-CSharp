using MartinZottmann.Engine.Graphics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Text;

namespace MartinZottmann.Engine.UnitTests
{
    [TestClass]
    public class Graphics_Camera
    {
        [TestMethod]
        public void LookAt()
        {
            using (var window = new GameWindow())
                for (var x0 = -1; x0 <= 1; x0++)
                    for (var y0 = -1; y0 <= 1; y0++)
                        for (var z0 = -1; z0 <= 1; z0++)
                            for (var x1 = -1; x1 <= 1; x1++)
                                for (var y1 = -1; y1 <= 1; y1++)
                                    for (var z1 = -1; z1 <= 1; z1++)
                                        LookAt(window, new Vector3d(x0, y0, z0), new Vector3d(x1, y1, z1));
        }

        protected void LookAt(GameWindow window, Vector3d position, Vector3d direction)
        {
            if (position == direction)
                return;

            // Arrange
            direction.Normalize();
            var camera = new Camera(window);
            camera.Position = position;

            // Act
            camera.LookAt = position + direction;

            // Assert
            AssertEquals(camera.ForwardRelative, direction, 0.0001, camera.Position, camera.LookAt);
        }

        protected void AssertEquals(Vector3d left, Vector3d right, double delta, params object[] list)
        {
            if (
                Math.Abs(left.X - right.X) > delta
                || Math.Abs(left.Y - right.Y) > delta
                || Math.Abs(left.Z - right.Z) > delta
            )
            {
                var sb = new StringBuilder();
                sb.Append(String.Format("{0} != {1} ({2})", left, right, delta));
                for (var i = 0; i < list.Length; i++)
                    sb.Append("; " + i + ": {" + i + "}");

                throw new AssertFailedException(String.Format(sb.ToString(), list));
            }
        }
    }
}
