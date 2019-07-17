using System;
using System.Threading.Tasks;

namespace Jal.Router.Model
{
    public class RouteMethod<TContent, TConsumer>
    {
        public RouteMethod(Func<TContent, TConsumer, Task> consumer, string status=null)
        {
            Consumer = consumer;
            Status = status;
        }

        public RouteMethod(Func<TContent, TConsumer, object, Task> consumer, string status = null)
        {
            ConsumerWithData = consumer;
            Status = status;
        }

        public RouteMethod(Func<TContent, TConsumer, MessageContext, Task> consumer, string status = null)
        {
            ConsumerWithContext = consumer;
            Status = status;
        }

        public RouteMethod(Func<TContent, TConsumer, MessageContext, object, Task> consumer, string status = null)
        {
            ConsumerWithDataAndContext = consumer;
            Status = status;
        }

        public string Status { get; private set; }

        public Func<TContent, TConsumer, Task> Consumer { get;  }

        public Func<TContent, TConsumer, bool> Evaluator { get; private set; }

        public Func<TContent, TConsumer, MessageContext, Task> ConsumerWithContext { get; }

        public Func<TContent, TConsumer, object, Task> ConsumerWithData { get; }

        public Func<TContent, TConsumer, MessageContext, object, Task> ConsumerWithDataAndContext { get; }

        public Func<TContent, TConsumer, MessageContext, bool> EvaluatorWithContext { get; private set; }

        public void UpdateEvaluator(Func<TContent, TConsumer, bool> evaluator)
        {
            Evaluator = evaluator;
        }

        public void UpdateEvaluatorWithContext(Func<TContent, TConsumer, MessageContext, bool> evaluatorwithcontext)
        {
            EvaluatorWithContext = evaluatorwithcontext;
        }
    }
}