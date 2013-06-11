namespace MartinZottmann.Graphics
{
    public interface IVertices<V> where V : struct
    {
        V[] Vertices { get; set; }

        int VerticesLength { get; }
    }
}
