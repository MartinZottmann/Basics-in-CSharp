using MartinZottmann.Engine.Graphics.Shapes;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Helper;
using OpenTK;

namespace MartinZottmann.Game.Entities
{
    class Textured : Physical, INavigation
    {
        Engine.Graphics.OpenGL.Entity graphic;

        public Vector3d Target { get; set; }

        protected const double max_speed = 20;

        protected const double slowing_distance = 40;

        public Textured(ResourceManager resources)
            : base(resources)
        {
            Scale *= 2;

            var shape = new Quad();
            for (var i = 0; i < shape.VerticesLength; i++)
                shape.Vertices[i].position = new Vector3(shape.Vertices[i].position.X, 0, -shape.Vertices[i].position.Y);

            graphic = new Engine.Graphics.OpenGL.Entity();
            graphic.Add(shape);
            graphic.Program = Resources.Programs["plain_texture"];
            graphic.Texture = Resources.Textures["res/textures/pointer.png"];

            graphic.Program.UniformLocations["Texture"].Set(0);

            BoundingBox.Max = Scale;
            BoundingBox.Min = -Scale;
        }

        public override void Update(double delta_time)
        {
            var rotation_angle = Vector3d.Dot(Orientation.Xyz, Target - Position);
            if (rotation_angle > System.Double.Epsilon)
            {
                var rotation_axis = Vector3d.UnitY; // Cross(Orientation.Xyz, Target - Position);
                rotation_axis.Normalize();
                var rotation = Quaterniond.FromAxisAngle(rotation_axis, rotation_angle);
                rotation.Normalize();
                AngularForce = rotation.Xyz - AngularVelocity;
            }
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
