using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class  AbstractRequestRouterInterceptor : IRequestRouterInterceptor
    {
        public static IRequestRouterInterceptor Instance = new NullRequestRouterInterceptor();


        public void OnEntry<TRequest, TResponse>(TRequest request, Route<TRequest, TResponse> route)
        {
        }

        public void OnExit<TRequest, TResponse>(TRequest request, Route<TRequest, TResponse> route, TResponse response)
        {
        }

        public void OnSuccess<TRequest, TResponse>(TRequest request, Route<TRequest, TResponse> route, EndPoint endPoint, TResponse response)
        {
        }

        public void OnError<TRequest, TResponse>(TRequest request, Route<TRequest, TResponse> route, TResponse response, Exception ex)
        {
        }
    }
}
