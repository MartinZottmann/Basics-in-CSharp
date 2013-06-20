using MartinZottmann.Engine.Graphics.Shapes;
using MartinZottmann.Engine.Resources;
using OpenTK;

namespace MartinZottmann.Game.Entities
{
    class Textured : Physical
    {
        Engine.Graphics.OpenGL.Entity graphic;

        public Vector3d Target;

        protected const double max_speed = 20;

        protected const double slowing_distance = 40;

        public Textured(ResourceManager resources)
            : base(resources)
        {
            Scale *= 2;
            rotate_x = -MathHelper.PiOver2;

            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Add(new Quad());
            graphic.Program = Resources.Programs["plain_texture"];
            graphic.Texture = Resources.Textures["res/textures/pointer.png"];

            graphic.Program.AddUniformLocation("Texture").Set(0);
            graphic.Program.AddUniformLocation("PVM");

            BoundingBox.Max = Scale;
            BoundingBox.Min = -Scale;
        }

        public override void Update(double delta_time)
        {
            //var direction = Target - Position;
            //var distance = direction.Length;
            //if (distance > System.Double.Epsilon)
            //{
            //    Force = (direction * (System.Math.Min(max_speed * (distance / slowing_distance), max_speed) / distance)) - Velocity;
            //}

            var direction = Target - Position;
            var distance = direction.Length;
            if (distance > System.Double.Epsilon)
            {
                direction /= distance;
                if (distance < slowing_distance)
                    Force = (direction * max_speed * (distance / slowing_distance)) - Velocity;
                else
                    Force = (direction * max_speed) - Velocity;
            }

            base.Update(delta_time);
        }

        public override void Render(double delta_time)
        {
            graphic.Program.UniformLocations["PVM"].Set(RenderContext.ProjectionViewModel);
            graphic.Draw();

#if DEBUG
            RenderVelocity(delta_time);
            RenderBoundingBox(delta_time);
#endif
        }
    }
}
