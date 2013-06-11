namespace MartinZottmann.Graphics
{
    public interface IVertex<V> where V : struct
    {
        V[] Vertices { get; set; }

        int VerticesLength { get; }
    }
}
