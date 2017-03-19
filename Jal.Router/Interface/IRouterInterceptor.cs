using System;

namespace Jal.Router.Interface
{
    public interface IRouterInterceptor
    {
        void OnEntry<TBody,TConsumer>(TBody message, TConsumer consumer);

        void OnExit<TBody, TConsumer>(TBody message, TConsumer consumer);

        void OnSuccess<TBody, TConsumer>(TBody message, TConsumer consumer);

        void OnError<TBody, TConsumer>(TBody message, TConsumer consumer, Exception ex);
    }
}