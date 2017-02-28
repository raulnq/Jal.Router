using System;

namespace Jal.Router.Interface
{
    public interface IMessagetRouterInterceptor
    {
        void OnEntry<TMessage>(TMessage message, IMessageSender<TMessage> sender);

        void OnExit<TMessage>(TMessage message, IMessageSender<TMessage> sender);

        void OnSuccess<TMessage>(TMessage message, IMessageSender<TMessage> sender);

        void OnError<TMessage>(TMessage message, IMessageSender<TMessage> sender, Exception ex);
    }
}