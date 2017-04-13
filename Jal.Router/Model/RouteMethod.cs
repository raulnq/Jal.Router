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

        public RouteMethod(Action<TBody, TConsumer, dynamic> consumer)
        {
            ConsumerWithContext = consumer;
        }

        public Action<TBody, TConsumer> Consumer { get; set; }

        public Func<TBody, TConsumer, bool> Evaluator { get; set; }

        public Action<TBody, TConsumer, dynamic> ConsumerWithContext { get; set; }

        public Func<TBody, TConsumer, dynamic, bool> EvaluatorWithContext { get; set; }
        public Type RetryExceptionType { get; set; }
        public Type RetryExtractorType { get; set; }
        public Func<IValueSettingFinder, IRetryPolicy> RetryPolicyExtractor { get; set; }
    }
}