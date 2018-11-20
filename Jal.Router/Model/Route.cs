﻿using System;
using System.Collections.Generic;
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
            IdentityConfiguration = new IdentityConfiguration();
            StorageConfiguration = new StorageConfiguration();
        }

        public Route(Saga saga, string name, Type contenttype, Type consumerinterfacetype) : this(name, contenttype, consumerinterfacetype)
        {
            Saga = saga;
        }

        public Saga Saga { get; }

        public Action<object> RuntimeHandler { get; set; }

        public IdentityConfiguration IdentityConfiguration { get; }

        public StorageConfiguration StorageConfiguration { get; }

        public List<Channel> Channels { get; }

        public Type ConsumerInterfaceType { get; }

        public string Name { get; set; }

        public Type ContentType { get; }

        public Type RetryExceptionType { get; set; }

        public Type RetryExtractorType { get; set; }

        public string OnRetryEndPoint { get; set; }

        public string OnErrorEndPoint { get; set; }

        public Func<IValueFinder, IRetryPolicy> RetryPolicyExtractor { get; set; }

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