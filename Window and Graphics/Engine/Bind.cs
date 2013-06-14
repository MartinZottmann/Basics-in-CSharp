using System;

namespace MartinZottmann.Engine
{
    public class Bind : IDisposable
    {
        protected readonly IBindable context;

        public Bind(IBindable target)
        {
            context = target;
            if (context != null)
                context.Bind();
        }

        public void Dispose()
        {
            if (context != null)
                context.UnBind();
        }
    }
}