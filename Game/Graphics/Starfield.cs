using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Game.Graphics
{
    public class Starfield : Model
    {
        public static Random Random = new Random();

        const int num_stars = 100000;

        public Starfield()
        {
            var vertex_data = new VertexP3C4[num_stars];
            for (int i = 0; i < num_stars; i++)
                vertex_data[i] = GetStar();

            Mesh(new Mesh<VertexP3C4, uint>(vertex_data));
            Mode = PrimitiveType.Points;
        }

        public void Update(double delta_time)
        {
            var i = Random.Next(0, Mesh().VerticesLength - 1);
            var bo = (BufferObject<VertexP3C4>)VertexArrayObject.BufferObjects[0];
            bo.Write(i, GetStar());
        }

        protected VertexP3C4 GetStar()
        {
            return new VertexP3C4(
                Random.Next(-1000, 1000),
                Random.Next(-1000, 1000),
                Random.Next(-1000, 1000),
                (float)Random.NextDouble(),
                (float)Random.NextDouble(),
                (float)Random.NextDouble(),
                (float)Random.NextDouble()
            );
        }
    }
}
