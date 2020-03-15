using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface ITypedConsumer
    {
        Task Consume<TContent, THandler>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class;

        Task Consume<TContent>(MessageContext context, TContent content, RouteMethod<TContent> routemethod);

        Task Consume<TContent, THandler, TData>(MessageContext context, TContent content, RouteMethodWithData<TContent, THandler, TData> routemethod, THandler handler, TData data) where THandler : class
            where TData : class, new();

        Task Consume<TContent, TData>(MessageContext context, TContent content, RouteMethodWithData<TContent, TData> routemethod, TData data) where TData : class, new();

        bool Select<TContent, THandler>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class;

        bool Select<TContent>(MessageContext context, TContent content, RouteMethod<TContent> routemethod);

        bool Select<TContent, THandler, TData>(MessageContext context, TContent content, RouteMethodWithData<TContent, THandler, TData> routemethod, THandler handler, TData data) where THandler : class;

        bool Select<TContent, TData>(MessageContext context, TContent content, RouteMethodWithData<TContent, TData> routemethod, TData data);
    }
}