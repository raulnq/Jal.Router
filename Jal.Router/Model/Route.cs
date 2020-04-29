using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class Route
    {
        public Route(string name, Type middleware)
        {
            Middlewares = new List<Type>();
            Name = name;
            Channels = new List<Channel>();
            ErrorHandlers = new List<ErrorHandler>();
            EntryHandlers = new List<Handler>();
            ExitHandlers = new List<Handler>();
            RouteMethods = new List<RouteMethod>();
            Middleware = middleware;
        }

        public override string ToString()
        {
            return Saga == null ? Name : $"{Saga.Name}/{Name}";
        }

        public Route(Saga saga, string name, Type middleware) : this(name, middleware)
        {
            Saga = saga;
        }

        public IList<ErrorHandler> ErrorHandlers { get; }

        public IList<Handler> EntryHandlers { get; }

        public IList<Handler> ExitHandlers { get; }

        public Saga Saga { get; }

        public List<Channel> Channels { get; }

        public string Name { get; private set; }

        public List<Type> Middlewares { get; }

        public Type Middleware { get; }

        public Func<MessageContext, bool> Condition { get; private set; }

        public RouteEntity ToEntity()
        {
            return new RouteEntity(Name);
        }

        public void When(Func<MessageContext, bool> condition)
        {
            Condition = condition;
        }

        public List<RouteMethod> RouteMethods { get; }

        public bool AnyRouteMethods()
        {
            return RouteMethods != null && RouteMethods.Count > 0;
        }
    }
}