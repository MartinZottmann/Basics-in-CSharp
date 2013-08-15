using OpenTK;

namespace MartinZottmann.Game.Entities.Components
{
    public class Target : Abstract
    {
        public Vector3d? Position;

        public Target(GameObject game_object) : base(game_object) { }
    }
}
