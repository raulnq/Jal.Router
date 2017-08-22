using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class RoutePicker : IRoutePicker
    {
        public bool Pick<TContent, THandler>(InboundMessageContext<TContent> context, RouteMethod<TContent, THandler> routemethod, THandler handler) where THandler : class
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