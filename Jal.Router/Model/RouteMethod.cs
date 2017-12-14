using System;

namespace Jal.Router.Model
{
    public class RouteMethod<TContent, TConsumer>
    {
        public RouteMethod(Action<TContent, TConsumer> consumer)
        {
            Consumer = consumer;
        }

        public RouteMethod(Action<TContent, TConsumer, object> consumer)
        {
            ConsumerWithData = consumer;
        }

        public RouteMethod(Action<TContent, TConsumer, MessageContext> consumer)
        {
            ConsumerWithContext = consumer;
        }

        public RouteMethod(Action<TContent, TConsumer, MessageContext, object> consumer)
        {
            ConsumerWithDataAndContext = consumer;
        }

        public Action<TContent, TConsumer> Consumer { get; set; }

        public Func<TContent, TConsumer, bool> Evaluator { get; set; }

        public Action<TContent, TConsumer, MessageContext> ConsumerWithContext { get; set; }

        public Action<TContent, TConsumer, object> ConsumerWithData { get; set; }

        public Action<TContent, TConsumer, MessageContext, object> ConsumerWithDataAndContext { get; set; }

        public Func<TContent, TConsumer, MessageContext, bool> EvaluatorWithContext { get; set; }
    }
}