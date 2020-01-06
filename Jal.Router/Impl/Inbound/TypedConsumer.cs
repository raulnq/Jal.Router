using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class TypedConsumer : ITypedConsumer
    {
        public Task Consume<TContent, THandler>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class
        {
            if (routemethod.ConsumerWithContext != null)
            {
                return routemethod.ConsumerWithContext(content, handler, context);
            }
            else
            {
                if (routemethod.Consumer != null)
                {
                    return routemethod.Consumer(content, handler);
                }
            }

            return Task.CompletedTask;
        }

        public Task Consume<TContent, THandler, TData>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler, TData data) where THandler : class
    where TData : class, new()
        {
            if (routemethod.ConsumerWithContext != null)
            {
                return routemethod.ConsumerWithContext(content, handler, context);
            }
            else
            {
                if (routemethod.Consumer != null)
                {
                    return routemethod.Consumer(content, handler);
                }
                else
                {
                    if (routemethod.ConsumerWithDataAndContext != null)
                    {
                        return routemethod.ConsumerWithDataAndContext(content, handler, context, data);
                    }
                    else
                    {
                        return routemethod.ConsumerWithData?.Invoke(content, handler, data);
                    }
                }
            }
        }

        public bool Select<TContent, THandler>(MessageContext context, TContent content, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class
        {
            if (routemethod.EvaluatorWithContext == null)
            {
                if (routemethod.Evaluator == null)
                {
                    return true;
                }
                else
                {
                    return routemethod.Evaluator(content, handler);
                }
            }
            else
            {
                return routemethod.EvaluatorWithContext(content, handler, context);
            }
        }
    }
}