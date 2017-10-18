using System;
using System.Collections.Generic;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;

namespace Jal.Router.Model
{
    public class Route
    {
        public Route(Type bodytype, Type consumerinterfacetype, string name="")
        {
            BodyType = bodytype;
            ConsumerInterfaceType = consumerinterfacetype;
            Name = name;
            FilterTypes = new List<Type>();
        }
        public Type ConsumerInterfaceType { get; set; }

        public string Name { get; set; }

        public Type BodyType { get; set; }

        public Type RetryExceptionType { get; set; }

        public Type RetryExtractorType { get; set; }

        public string OnRetryEndPoint { get; set; }

        public string OnErrorEndPoint { get; set; }

        public Func<IValueSettingFinder, IRetryPolicy> RetryPolicyExtractor { get; set; }

        public IList<Type> FilterTypes { get; set; }

        public string ForwardEndPoint { get; set; }
    }

    public class Route<TBody, TConsumer> : Route
    {
        public List<RouteMethod<TBody, TConsumer>> RouteMethods { get; set; }

        public Func<TBody, MessageContext, bool> When { get; set; }

        public Type ConsumerType { get; set; }

        public Route(string name = "") : base(typeof(TBody), typeof(TConsumer), name)
        {
            RouteMethods = new List<RouteMethod<TBody, TConsumer>>();
        }
    }
}