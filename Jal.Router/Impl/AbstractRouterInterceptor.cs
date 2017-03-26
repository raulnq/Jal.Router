using System;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public abstract class  AbstractRouterInterceptor : IRouterInterceptor
    {
        public static IRouterInterceptor Instance = new NullRouterInterceptor();

        public virtual void OnEntry<TContent, THandler>(TContent content, THandler handler)
        {
        }

        public virtual void OnExit<TContent, THandler>(TContent content, THandler handler)
        {
        }

        public virtual void OnSuccess<TContent, THandler>(TContent content, THandler handler)
        {
        }

        public virtual void OnError<TContent, THandler>(TContent content, THandler handler, Exception ex)
        {
        }
    }
}
