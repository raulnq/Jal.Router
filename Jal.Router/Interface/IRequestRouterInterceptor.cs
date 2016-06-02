using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRequestRouterInterceptor
    {
        void OnEntry<TRequest, TResponse>(TRequest request, Route<TRequest, TResponse> route);

        void OnExit<TRequest, TResponse>(TRequest request, Route<TRequest, TResponse> route, TResponse response);

        void OnSuccess<TRequest, TResponse>(TRequest request, Route<TRequest, TResponse> route, Model.EndPoint endPoint, TResponse response);

        void OnError<TRequest, TResponse>(TRequest request, Route<TRequest, TResponse> route, TResponse response, Exception ex);
    }
}