namespace Jal.Router.Interface
{
    public interface IRequestRouter
    {
        TResponse[] Route<TRequest, TResponse>(TRequest request);

        TResponse[] Route<TRequest, TResponse>(TRequest request, string route);

        IRouteProvider Provider { get; }

        IRequestRouterInterceptor Interceptor { get; set; }
    }
}