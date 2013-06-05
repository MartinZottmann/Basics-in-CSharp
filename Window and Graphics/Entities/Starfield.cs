using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace MartinZottmann.Entities
{
    class Starfield : Entity
    {
        const int num_stars = 10000;

        int[,] stars;

        public Starfield()
            : base()
        {
            color = Color.LightGray;

            stars = new int[num_stars, 7];
            for (int i = 0; i < num_stars; i++)
            {
                stars[i, 0] = randomNumber.Next(-1000, 1000);
                stars[i, 1] = randomNumber.Next(-1000, 1000);
                stars[i, 2] = randomNumber.Next(-1000, 1000);
                stars[i, 3] = randomNumber.Next(1, 5);
            }
        }

        public override void Render(double delta_time)
        {
            for (int i = 0; i < num_stars; i++)
            {
                GL.PushMatrix();
                {
                    GL.PointSize(stars[i, 3]);
                    GL.Begin(BeginMode.Points);
                    {
                        GL.Color3(color);
                        GL.Vertex3(stars[i, 0], stars[i, 1], stars[i, 2]);
                    }
                    GL.End();
                }
                GL.PopMatrix();
            }

        }
    }
}
