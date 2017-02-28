using System;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public abstract class  AbstractMessagetRouterInterceptor : IMessagetRouterInterceptor
    {
        public static IMessagetRouterInterceptor Instance = new NullMessagetRouterInterceptor();


        public void OnEntry<TMessage>(TMessage message, IMessageSender<TMessage> sender)
        {
        }

        public void OnExit<TMessage>(TMessage message, IMessageSender<TMessage> sender)
        {
        }

        public void OnSuccess<TMessage>(TMessage message, IMessageSender<TMessage> sender)
        {
        }

        public void OnError<TMessage>(TMessage message, IMessageSender<TMessage> sender, Exception ex)
        {
        }
    }
}
