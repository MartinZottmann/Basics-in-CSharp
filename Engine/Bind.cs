using System;

namespace MartinZottmann.Engine
{
    public class Bind : IDisposable
    {
        protected IBindable context;

        public Bind(IBindable target)
        {
            context = target;
            if (null != context)
                context.Bind();
        }

        ~Bind()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != context)
                {
                    context.UnBind();
                    context = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}