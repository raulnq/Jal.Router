using System;

namespace Jal.Router.Interface
{
    public interface IMessagetRouterInterceptor
    {
        void OnEntry<TMessage>(TMessage message, IMessageHandler<TMessage> handler);

        void OnExit<TMessage>(TMessage message, IMessageHandler<TMessage> handler);

        void OnSuccess<TMessage>(TMessage message, IMessageHandler<TMessage> handler);

        void OnError<TMessage>(TMessage message, IMessageHandler<TMessage> handler, Exception ex);
    }
}