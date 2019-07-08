using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jal.Router.Model
{
    public class Route
    {
        public Route(string name, Type contenttype, Type consumerinterfacetype)
        {
            ContentType = contenttype;
            ConsumerInterfaceType = consumerinterfacetype;
            MiddlewareTypes = new List<Type>();
            Name = name;
            Channels = new List<Channel>();
            ErrorHandlers = new List<ErrorHandler>();
            EntryHandlers = new List<Handler>();
            ExitHandlers = new List<Handler>();
        }

        public Route(Saga saga, string name, Type contenttype, Type consumerinterfacetype) : this(name, contenttype, consumerinterfacetype)
        {
            Saga = saga;
        }

        public IList<ErrorHandler> ErrorHandlers { get; }

        public IList<Handler> EntryHandlers { get; }

        public IList<Handler> ExitHandlers { get; }

        public Saga Saga { get; }

        public Func<object, Channel, Task> RuntimeHandler { get; set; }

        public List<Channel> Channels { get; }

        public Type ConsumerInterfaceType { get; }

        public string Name { get; set; }

        public Type ContentType { get; }

        public List<Type> MiddlewareTypes { get; }

        public Func<MessageContext, bool> When { get; set; }

        public bool UseClaimCheck { get; set; }

        public RouteEntity ToEntity()
        {
            return new RouteEntity(Name, ContentType);
        }
    }

    public class Route<TContent, TConsumer> : Route
    {
        public List<RouteMethod<TContent, TConsumer>> RouteMethods { get; }

        public Type ConsumerType { get; set; }

        public Route(string name) : base(name, typeof(TContent), typeof(TConsumer))
        {
            RouteMethods = new List<RouteMethod<TContent, TConsumer>>();
        }

        public Route(Saga saga, string name) : base(saga, name, typeof(TContent), typeof(TConsumer))
        {
            RouteMethods = new List<RouteMethod<TContent, TConsumer>>();
        }
    }
}