using System;
using System.Threading.Tasks;

namespace Jal.Router.Model
{
    public class RouteMethod<TContent, TConsumer>
    {
        public RouteMethod(Func<TContent, TConsumer, Task> consumer)
        {
            Consumer = consumer;
        }

        public RouteMethod(Func<TContent, TConsumer, object, Task> consumer)
        {
            ConsumerWithData = consumer;
        }

        public RouteMethod(Func<TContent, TConsumer, MessageContext, Task> consumer)
        {
            ConsumerWithContext = consumer;
        }

        public RouteMethod(Func<TContent, TConsumer, MessageContext, object, Task> consumer)
        {
            ConsumerWithDataAndContext = consumer;
        }

        public string Status { get; set; }

        public Func<TContent, TConsumer, Task> Consumer { get;  }

        public Func<TContent, TConsumer, bool> Evaluator { get; set; }

        public Func<TContent, TConsumer, MessageContext, Task> ConsumerWithContext { get; }

        public Func<TContent, TConsumer, object, Task> ConsumerWithData { get; }

        public Func<TContent, TConsumer, MessageContext, object, Task> ConsumerWithDataAndContext { get; }

        public Func<TContent, TConsumer, MessageContext, bool> EvaluatorWithContext { get; set; }
    }
}