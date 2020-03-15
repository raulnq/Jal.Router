using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class TypedConsumer : ITypedConsumer
    {
        public Task Consume<TContent, THandler>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class
        {
            if (routemethod.Consumer != null)
            {
                return routemethod.Consumer(content, handler, context);
            }

            return Task.CompletedTask;
        }

        public Task Consume<TContent>(MessageContext context, TContent content, RouteMethod<TContent> routemethod)
        {
            if (routemethod.Consumer != null)
            {
                return routemethod.Consumer(content, context);
            }

            return Task.CompletedTask;
        }

        public Task Consume<TContent, THandler, TData>(MessageContext context, TContent content, RouteMethodWithData<TContent, THandler, TData> routemethod, THandler handler, TData data) where THandler : class
            where TData : class, new()
        {
            if (routemethod.Consumer != null)
            {
                return routemethod.Consumer(content, handler, context, data);
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        public Task Consume<TContent, TData>(MessageContext context, TContent content, RouteMethodWithData<TContent, TData> routemethod, TData data)
            where TData : class, new()
        {
            if (routemethod.Consumer != null)
            {
                return routemethod.Consumer(content, context, data);
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        public bool Select<TContent, THandler>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class
        {
            if (routemethod.Evaluator == null)
            {
                return true;
            }
            else
            {
                return routemethod.Evaluator(content, handler, context);
            }
        }

        public bool Select<TContent>(MessageContext context, TContent content, RouteMethod<TContent> routemethod)
        {
            if (routemethod.Evaluator == null)
            {
                return true;
            }
            else
            {
                return routemethod.Evaluator(content, context);
            }
        }

        public bool Select<TContent, THandler, TData>(MessageContext context, TContent content, RouteMethodWithData<TContent, THandler, TData> routemethod, THandler handler, TData data) where THandler : class
        {
            if (routemethod.Evaluator == null)
            {
                return true;
            }
            else
            {
                return routemethod.Evaluator(content, handler, context, data);
            }
        }

        public bool Select<TContent, TData>(MessageContext context, TContent content, RouteMethodWithData<TContent, TData> routemethod, TData data)
        {
            if (routemethod.Evaluator == null)
            {
                return true;
            }
            else
            {
                return routemethod.Evaluator(content, context, data);
            }
        }

    }
}