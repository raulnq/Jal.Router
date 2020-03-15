using System;
using System.Threading.Tasks;

namespace Jal.Router.Model
{
    public class RouteMethod
    {
        public Type HandlerType { get; private set;  }

        public bool IsAnonymous { get; private set; }

        protected RouteMethod(Type handlertype)
        {
            HandlerType = handlertype;
            IsAnonymous = false;
        }

        protected RouteMethod()
        {
            IsAnonymous = true;
        }
    }

    public class RouteMethod<TContent> : RouteMethod
    {
        public Func<TContent, MessageContext, Task> Consumer { get; }

        public Func<TContent, MessageContext, bool> Evaluator { get; private set; }

        public RouteMethod(Func<TContent, MessageContext, Task> consumer) : base()
        {
            Consumer = consumer;
        }

        public void UpdateEvaluator(Func<TContent, MessageContext, bool> evaluator)
        {
            Evaluator = evaluator;
        }
    }

    public class RouteMethod<TContent, THandler> : RouteMethod
    {
        public Type ConcreteHandlerType { get; }

        protected RouteMethod(Type handlertype, Type concretehandlertype): base(handlertype)
        {
            ConcreteHandlerType = concretehandlertype;
        }

        public RouteMethod(Func<TContent, THandler, MessageContext, Task> consumer, Type concreteconsumertype) : this(typeof(THandler), concreteconsumertype)
        {
            Consumer = consumer;
        }

        public Func<TContent, THandler, MessageContext, Task> Consumer { get; }

        public Func<TContent, THandler, MessageContext, bool> Evaluator { get; private set; }

        public void UpdateEvaluator(Func<TContent, THandler, MessageContext, bool> evaluator)
        {
            Evaluator = evaluator;
        }
    }
}