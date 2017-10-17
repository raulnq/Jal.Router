using System;

namespace Jal.Router.Model
{
    public class RouteMethod<TBody, TConsumer>
    {
        public RouteMethod(Action<TBody, TConsumer> consumer)
        {
            Consumer = consumer;
        }

        public RouteMethod(Action<TBody, TConsumer, object> consumer)
        {
            ConsumerWithData = consumer;
        }

        public RouteMethod(Action<TBody, TConsumer, MessageContext> consumer)
        {
            ConsumerWithContext = consumer;
        }

        public RouteMethod(Action<TBody, TConsumer, MessageContext, object> consumer)
        {
            ConsumerWithDataAndContext = consumer;
        }

        public Action<TBody, TConsumer> Consumer { get; set; }

        public Func<TBody, TConsumer, bool> Evaluator { get; set; }

        public Action<TBody, TConsumer, MessageContext> ConsumerWithContext { get; set; }

        public Action<TBody, TConsumer, object> ConsumerWithData { get; set; }

        public Action<TBody, TConsumer, MessageContext, object> ConsumerWithDataAndContext { get; set; }

        public Func<TBody, TConsumer, MessageContext, bool> EvaluatorWithContext { get; set; }
    }
}