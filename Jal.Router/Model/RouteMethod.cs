using System;
using Jal.Router.Interface;

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

        public RouteMethod(Action<TBody, TConsumer, InboundMessageContext> consumer)
        {
            ConsumerWithContext = consumer;
        }

        public RouteMethod(Action<TBody, TConsumer, InboundMessageContext, object> consumer)
        {
            ConsumerWithDataAndContext = consumer;
        }

        public Action<TBody, TConsumer> Consumer { get; set; }

        public Func<TBody, TConsumer, bool> Evaluator { get; set; }

        public Action<TBody, TConsumer, InboundMessageContext> ConsumerWithContext { get; set; }

        public Action<TBody, TConsumer, object> ConsumerWithData { get; set; }

        public Action<TBody, TConsumer, InboundMessageContext, object> ConsumerWithDataAndContext { get; set; }

        public Func<TBody, TConsumer, InboundMessageContext, bool> EvaluatorWithContext { get; set; }
        public Type RetryExceptionType { get; set; }
        public Type RetryExtractorType { get; set; }
        public Func<IValueSettingFinder, IRetryPolicy> RetryPolicyExtractor { get; set; }
    }
}