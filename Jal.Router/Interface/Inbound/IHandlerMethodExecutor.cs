using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface.Inbound
{
    public interface IHandlerMethodExecutor
    {
        Task Execute<TContent, THandler>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class;

        Task Execute<TContent, THandler, TData>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler, TData data) where THandler : class
            where TData : class, new();
    }
}