namespace MartinZottmann.Engine.Graphics.Mesh
{
    public interface IIndices<I> where I : struct
    {
        I[] Indices { get; set; }

        int IndicesLength { get; }
    }
}
