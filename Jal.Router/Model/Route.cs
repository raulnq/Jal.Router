using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model.Management;

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
            IdentityConfiguration = new Identity();
            StorageConfiguration = new Storage();
            RetryExceptionTypes = new List<Type>();
        }

        public Route(Saga saga, string name, Type contenttype, Type consumerinterfacetype) : this(name, contenttype, consumerinterfacetype)
        {
            Saga = saga;
        }

        public List<Type> RetryExceptionTypes { get; }

        public Type RetryValueFinderType { get; set; }

        public string OnRetryEndPoint { get; set; }

        public Func<IValueFinder, IRetryPolicy> RetryPolicyProvider { get; set; }

        public Saga Saga { get; }

        public Func<object, Channel, Task> RuntimeHandler { get; set; }

        public Identity IdentityConfiguration { get; }

        public Storage StorageConfiguration { get; }

        public List<Channel> Channels { get; }

        public Type ConsumerInterfaceType { get; }

        public string Name { get; set; }

        public Type ContentType { get; }

        public string OnErrorEndPoint { get; set; }

        public List<Type> MiddlewareTypes { get; }

        public string ForwardEndPoint { get; set; }

        public Func<MessageContext, bool> When { get; set; }

        public bool UseClaimCheck { get; set; }
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