using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class EndPointBuilder : IToEndPointBuilder, IOnEndPointOptionBuilder, IForMessageEndPointBuilder, IToChannelBuilder, IToReplyChannelBuilder
    {
        private EndPoint _endpoint;

        private readonly List<EndPoint> _endpoints;

        private readonly string _name;

        public EndPointBuilder(List<EndPoint> endpoints, string name)
        {
            _endpoints = endpoints;
            _name = name;
        }

        public IToEndPointBuilder ForMessage<TMessage>()
        {
            _endpoint = new EndPoint(_name, typeof(TMessage));

            _endpoints.Add(_endpoint);

            return this;
        }

        public void AddPointToPointChannel(string connectionstring, string path, Type adapter = null, Type type = null)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (adapter != null && !typeof(IMessageAdapter).IsAssignableFrom(adapter))
            {
                throw new InvalidOperationException("The adapter type is not valid");
            }

            if (type != null && !typeof(IPointToPointChannel).IsAssignableFrom(type))
            {
                throw new InvalidOperationException("The channel type is not valid");
            }

            var channel = new Channel(ChannelType.PointToPoint, connectionstring, path, adapter, type);

            _endpoint.Channels.Add(channel);
        }

        IAndWaitReplyFromEndPointBuilder IToReplyChannelBuilder.AddPointToPointChannel(string connectionstring, string path) 
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var channel = new Channel(ChannelType.PointToPoint, connectionstring, path, null, null);

            _endpoint.Channels.Add(channel);

            return new AndWaitReplyFromEndPointBuilder(channel);
        }

        public void AddPublishSubscribeChannel(string connectionstring, string path, Type adapter = null, Type type = null)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (adapter != null && !typeof(IMessageAdapter).IsAssignableFrom(adapter))
            {
                throw new InvalidOperationException("The adapter type is not valid");
            }

            if (type != null && !typeof(IPublishSubscribeChannel).IsAssignableFrom(type))
            {
                throw new InvalidOperationException("The channel type is not valid");
            }

            var channel = new Channel(ChannelType.PublishSubscribe, connectionstring, path, adapter, type);

            _endpoint.Channels.Add(channel);
        }

        public IOnEndPointOptionBuilder To(Action<IToChannelBuilder> channelbuilder)
        {
            if (channelbuilder==null)
            {
                throw new ArgumentNullException(nameof(channelbuilder));
            }

            channelbuilder(this);

            return this;
        }

        public IOnEndPointOptionBuilder UseMiddleware(Action<IEndpointMiddlewareBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new EndpointMiddlewareBuilder(_endpoint);

            action(builder);

            return this;
        }

        public IOnEndPointOptionBuilder To<TReply>(Action<IToReplyChannelBuilder> channelbuilder)
        {
            if (channelbuilder == null)
            {
                throw new ArgumentNullException(nameof(channelbuilder));
            }

            _endpoint.SetReplyContentType(typeof(TReply));

            channelbuilder(this);

            return this;
        }

        public IOnEndPointOptionBuilder OnError(Action<IOnEndPointErrorBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new OnEndPointErrorBuilder(_endpoint);

            action(builder);

            return this;
        }

        public IOnEndPointOptionBuilder OnEntry(Action<IOnEndPointEntryBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new OnEndPointEntryBuilder(_endpoint);

            action(builder);

            return this;
        }

        public IOnEndPointOptionBuilder OnExit(Action<IOnEndPointExitBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new OnEndPointExitBuilder(_endpoint);

            action(builder);

            return this;
        }

        public void With(Action<IOnEndPointWithBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new OnEndPointWithBuilder(_endpoint);

            action(builder);
        }

        public IOnEndPointOptionBuilder When(Func<EndPoint, Options, Type, bool> condition)
        {
            _endpoint.When(condition);

            return this;
        }
    }
}