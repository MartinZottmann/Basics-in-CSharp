namespace MartinZottmann.Engine.Graphics
{
    public interface IIndices<I> where I : struct
    {
        I[] Indices { get; set; }

        int IndicesLength { get; }
    }
}
