using System;

namespace MartinZottmann.Engine
{
    public class Bind : IDisposable
    {
        protected readonly IBindable context;

        public Bind(IBindable target)
        {
            context = target;
            if (null != context)
                context.Bind();
        }

        public void Dispose()
        {
            if (null != context)
                context.UnBind();
        }
    }
}