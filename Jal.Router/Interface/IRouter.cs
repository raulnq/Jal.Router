namespace Jal.Router.Interface
{
    public interface IRouter
    {
        void Route<TContent>(TContent content, string name="");

        void Route<TContent>(TContent content, dynamic context, string name = "");

        IHandlerFactory Factory { get; }

        IRouteProvider Provider { get;}

        IRouterInterceptor Interceptor { get; }
    }
}