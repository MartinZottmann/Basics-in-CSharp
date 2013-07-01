namespace MartinZottmann.Engine.Graphics
{
    public interface IVertices<V> where V : struct, IVertex
    {
        V[] Vertices { get; set; }

        int VerticesLength { get; }
    }
}
