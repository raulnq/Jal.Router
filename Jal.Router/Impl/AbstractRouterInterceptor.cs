using System;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public abstract class  AbstractRouterInterceptor : IRouterInterceptor
    {
        public static IRouterInterceptor Instance = new NullRouterInterceptor();

        public virtual void OnEntry<TBody, TConsumer>(TBody message, TConsumer consumer)
        {
        }

        public virtual void OnExit<TBody, TConsumer>(TBody message, TConsumer consumer)
        {
        }

        public virtual void OnSuccess<TBody, TConsumer>(TBody message, TConsumer consumer)
        {
        }

        public virtual void OnError<TBody, TConsumer>(TBody message, TConsumer consumer, Exception ex)
        {
        }
    }
}
