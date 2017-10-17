using Jal.Router.Interface.Inbound;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound
{
    public class RouteMethodSelector : IRouteMethodSelector
    {
        public bool Select<TContent, THandler>(IndboundMessageContext<TContent> context, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class
        {
            if (routemethod.EvaluatorWithContext == null)
            {
                if (routemethod.Evaluator == null)
                {
                    return true;
                }
                else
                {
                    if (routemethod.Evaluator(context.Content, handler))
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (routemethod.EvaluatorWithContext(context.Content, handler, context))
                {
                    return true;
                }
            }

            return false;
        }
    }
}