using OpenTK;

namespace MartinZottmann.Engine.Physics
{
    public interface ICollider<T>
    {
        T At(Vector3d position_world);

        T At(ref Vector3d position_world);

        T At(Matrix4d model_world);

        T At(ref Matrix4d model_world);
    }
}
