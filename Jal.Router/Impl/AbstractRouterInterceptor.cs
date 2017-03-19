using System;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public abstract class  AbstractRouterInterceptor : IRouterInterceptor
    {
        public static IRouterInterceptor Instance = new NullRouterInterceptor();

        public void OnEntry<TBody, TConsumer>(TBody message, TConsumer consumer)
        {
        }

        public void OnExit<TBody, TConsumer>(TBody message, TConsumer consumer)
        {
        }

        public void OnSuccess<TBody, TConsumer>(TBody message, TConsumer consumer)
        {
        }

        public void OnError<TBody, TConsumer>(TBody message, TConsumer consumer, Exception ex)
        {
        }
    }
}
