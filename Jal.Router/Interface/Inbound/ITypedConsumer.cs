using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface ITypedConsumer
    {
        Task Consume<TContent, THandler>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class;

        Task Consume<TContent, THandler, TData>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler, TData data) where THandler : class
            where TData : class, new();

        bool Select<TContent, THandler>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class;

    }
}