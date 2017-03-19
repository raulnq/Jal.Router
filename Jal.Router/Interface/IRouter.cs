using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRouter
    {
        void Route<TBody>(TBody body, string name="");

        void Route<TBody>(TBody body, dynamic context, string name = "");

        IConsumerFactory Factory { get; }

        IRouteProvider Provider { get;}

        IRouterInterceptor Interceptor { get; }
    }
}