using System;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public abstract class  AbstractMessagetRouterInterceptor : IMessagetRouterInterceptor
    {
        public static IMessagetRouterInterceptor Instance = new NullMessagetRouterInterceptor();


        public void OnEntry<TMessage>(TMessage message, IMessageHandler<TMessage> handler)
        {
        }

        public void OnExit<TMessage>(TMessage message, IMessageHandler<TMessage> handler)
        {
        }

        public void OnSuccess<TMessage>(TMessage message, IMessageHandler<TMessage> handler)
        {
        }

        public void OnError<TMessage>(TMessage message, IMessageHandler<TMessage> handler, Exception ex)
        {
        }
    }
}
