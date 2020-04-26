using System;
using System.Threading.Tasks;

namespace Jal.Router.Model
{
    public class RouteMethod
    {
        public Type HandlerType { get; private set;  }

        public Type ContentType { get; private set; }

        public Func<MessageContext, bool> Condition { get; private set; }

        public bool IsAnonymous { get; private set; }

        protected RouteMethod(Type handlertype, Type contenttype, Func<MessageContext, bool> condition)
        {
            HandlerType = handlertype;
            IsAnonymous = false;
            ContentType = contenttype;
            Condition = condition;
        }

        protected RouteMethod(Type contenttype, Func<MessageContext, bool> condition)
        {
            IsAnonymous = true;
            ContentType = contenttype;
            Condition = condition;
        }
    }

    public class RouteMethod<TContent> : RouteMethod
    {
        public Func<TContent, MessageContext, Task> Consumer { get; }

        public Func<TContent, MessageContext, bool> Evaluator { get; private set; }

        public RouteMethod(Func<TContent, MessageContext, Task> consumer, Type contenttype, Func<MessageContext, bool> condition) 
            : base(contenttype, condition)
        {
            Consumer = consumer;
        }

        public void When(Func<TContent, MessageContext, bool> evaluator)
        {
            Evaluator = evaluator;
        }
    }

    public class RouteMethod<TContent, THandler> : RouteMethod
    {
        public Type ConcreteHandlerType { get; }

        protected RouteMethod(Type handlertype, Type concretehandlertype, Type contenttype, Func<MessageContext, bool> condition) 
            : base(handlertype, contenttype, condition)
        {
            ConcreteHandlerType = concretehandlertype;
        }

        public RouteMethod(Func<TContent, THandler, MessageContext, Task> consumer, Type concreteconsumertype, Type contenttype, Func<MessageContext, bool> condition) 
            : this(typeof(THandler), concreteconsumertype, contenttype, condition)
        {
            Consumer = consumer;
        }

        public Func<TContent, THandler, MessageContext, Task> Consumer { get; }

        public Func<TContent, THandler, MessageContext, bool> Evaluator { get; private set; }

        public void When(Func<TContent, THandler, MessageContext, bool> evaluator)
        {
            Evaluator = evaluator;
        }
    }
}