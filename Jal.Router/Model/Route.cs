using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class Route
    {
        public Route(Type bodytype, Type consumerinterfacetype, string name="")
        {
            BodyType = bodytype;
            ConsumerInterfaceType = consumerinterfacetype;
            Name = name;

        }
        public Type ConsumerInterfaceType { get; set; }

        public string Name { get; set; }

        public Type BodyType { get; set; }
    }

    public class Route<TBody, TConsumer> : Route
    {
        public List<RouteMethod<TBody, TConsumer>> RouteMethods { get; set; }

        public Func<TBody, InboundMessageContext, bool> When { get; set; }

        public Type ConsumerType { get; set; }

        public Route(string name = "") : base(typeof(TBody), typeof(TConsumer), name)
        {
            RouteMethods = new List<RouteMethod<TBody, TConsumer>>();
        }
    }
}