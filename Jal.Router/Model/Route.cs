using Jal.Router.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jal.Router.Model
{
    public class Route
    {
        public Route(string name, Type contenttype, List<Channel> channels)
        {
            ContentType = contenttype;
            MiddlewareTypes = new List<Type>();
            Name = name;
            Channels = channels;
            ErrorHandlers = new List<ErrorHandler>();
            EntryHandlers = new List<Handler>();
            ExitHandlers = new List<Handler>();
            RouteMethods = new List<RouteMethod>();
        }

        public Route(Saga saga, string name, Type contenttype, List<Channel> channels) : this(name, contenttype, channels)
        {
            Saga = saga;
        }

        public IList<ErrorHandler> ErrorHandlers { get; }

        public IList<Handler> EntryHandlers { get; }

        public IList<Handler> ExitHandlers { get; }

        public Saga Saga { get; }

        public Func<object, Channel, IMessageAdapter, Task> Consumer { get; private set; }

        public List<Channel> Channels { get; }

        public string Name { get; private set; }

        public Type ContentType { get; }

        public List<Type> MiddlewareTypes { get; }

        public Func<MessageContext, bool> When { get; private set; }

        public bool UseClaimCheck { get; private set; }

        public RouteEntity ToEntity()
        {
            return new RouteEntity(Name, ContentType);
        }

        public void UpdateUseClaimCheck(bool useclaimcheck)
        {
            UseClaimCheck = useclaimcheck;
        }

        public void SetConsumer(Func<object, Channel, IMessageAdapter, Task> consumer)
        {
            Consumer = consumer;
        }

        public void UpdateWhen(Func<MessageContext, bool> when)
        {
            When = when;
        }

        public List<RouteMethod> RouteMethods { get; }

        public bool AnyRouteMethods()
        {
            return RouteMethods != null && RouteMethods.Count > 0;
        }
    }
}