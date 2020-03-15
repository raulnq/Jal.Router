using System;
using System.Threading.Tasks;

namespace Jal.Router.Model
{
    public class RouteMethodWithData<TContent, TData> : RouteMethod
    {
        public string Status { get; private set; }

        public Func<TContent, MessageContext, TData, bool> Evaluator { get; private set; }

        public Func<TContent, MessageContext, TData, Task> Consumer { get; }

        public RouteMethodWithData(Func<TContent, MessageContext, TData, Task> consumer, string status = null) : base()
        {
            Consumer = consumer;
            Status = status;
        }

        public void UpdateEvaluator(Func<TContent, MessageContext, TData, bool> evaluator)
        {
            Evaluator = evaluator;
        }
    }

    public class RouteMethodWithData<TContent, THandler, TData> : RouteMethod
    {
        public string Status { get; private set; }

        protected RouteMethodWithData(Type handlertype, Type concretehandlertype) : base(handlertype)
        {
            ConcreteHandlerType = concretehandlertype;
        }

        public Type ConcreteHandlerType { get; }

        public Func<TContent, THandler, MessageContext, TData, Task> Consumer { get; }

        public Func<TContent, THandler, MessageContext, TData, bool> Evaluator { get; private set; }

        public RouteMethodWithData(Func<TContent, THandler, MessageContext, TData, Task> consumer, Type concreteconsumertype, string status = null) : this(typeof(THandler), concreteconsumertype)
        {
            Consumer = consumer;
            Status = status;
        }

        public void UpdateEvaluator(Func<TContent, THandler, MessageContext, TData, bool> evaluator)
        {
            Evaluator = evaluator;
        }
    }
}