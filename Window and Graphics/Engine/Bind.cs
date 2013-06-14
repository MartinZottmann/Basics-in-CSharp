using System;

namespace MartinZottmann.Engine
{
    public class Bind : IDisposable
    {
        protected readonly IBindable context;

        public Bind(IBindable target)
        {
            context = target;
            context.Bind();
        }

        public void Dispose()
        {
            context.UnBind();
        }
    }
}