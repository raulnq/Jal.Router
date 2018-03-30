using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public class HandlerMethodSelector : IHandlerMethodSelector
    {
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
                    if (routemethod.Evaluator(content, handler))
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (routemethod.EvaluatorWithContext(content, handler, context))
                {
                    return true;
                }
            }

            return false;
        }
    }
}