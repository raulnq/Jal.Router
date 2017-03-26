using System;

namespace Jal.Router.Interface
{
    public interface IRouterInterceptor
    {
        void OnEntry<TContent,THandler>(TContent content, THandler handler);

        void OnExit<TContent, THandler>(TContent content, THandler handler);

        void OnSuccess<TContent, THandler>(TContent content, THandler handler);

        void OnError<TContent, THandler>(TContent content, THandler handler, Exception ex);
    }
}