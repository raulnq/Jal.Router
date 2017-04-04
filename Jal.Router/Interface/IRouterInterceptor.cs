using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{


    //public interface IRouterInterceptor
    //{
    //    void OnEntry<TContent,THandler>(TContent content, THandler handler);

    //    void OnExit<TContent, THandler>(TContent content, THandler handler);

    //    void OnSuccess<TContent, THandler>(TContent content, THandler handler);

    //    void OnError<TContent, THandler>(TContent content, THandler handler, Exception ex);
    //}

    public interface IRouterInterceptor
    {
        void OnEntry(InboundMessageContext context);

        void OnSuccess<TContent>(InboundMessageContext context, TContent content);

        void OnExit(InboundMessageContext context);

        void OnException(InboundMessageContext context, Exception exception);
    }
}