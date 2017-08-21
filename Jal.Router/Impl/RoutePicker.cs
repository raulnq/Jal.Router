using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class RoutePicker : IRoutePicker
    {
        public bool Pick<TContent, THandler>(InboundMessageContext<TContent> context, RouteMethod<TContent, THandler> routeMethod, THandler consumer) where THandler : class
        {
            if (routeMethod.EvaluatorWithContext == null)
            {
                if (routeMethod.Evaluator == null)
                {
                    return true;
                }
                else
                {
                    if (routeMethod.Evaluator(context.Content, consumer))
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (routeMethod.EvaluatorWithContext(context.Content, consumer, context))
                {
                    return true;
                }
            }

            return false;
        }
    }
}