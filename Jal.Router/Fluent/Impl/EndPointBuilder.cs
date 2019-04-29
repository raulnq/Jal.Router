using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class EndPointBuilder : IOnEndPointOptionBuilder, INameEndPointBuilder, IToChannelBuilder, IToReplyChannelBuilder
    {
        private readonly EndPoint _endpoint;

        public EndPointBuilder(EndPoint endpoint)
        {
            _endpoint = endpoint;
        }

        public IOnEndPointOptionBuilder ForMessage<TMessage>()
        {
            _endpoint.MessageType = typeof (TMessage);

            return this;
        }

        public void AddPointToPointChannel<TValueFinder>(Func<IValueFinder, string> connectionstringprovider, string path) where TValueFinder : IValueFinder
        {
            var channel = new Channel(ChannelType.PointToPoint)
            {
                ConnectionStringValueFinderType = typeof(TValueFinder)
            };

            if (connectionstringprovider == null)
            {
                throw new ArgumentNullException(nameof(connectionstringprovider));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            channel.ToConnectionStringProvider = connectionstringprovider;

            channel.ToPath = path;

            _endpoint.Channels.Add(channel);
        }

        IAndWaitReplyFromEndPointBuilder IToReplyChannelBuilder.AddPointToPointChannel<TValueFinder>(Func<IValueFinder, string> connectionstringprovider, string path) //where TValueFinder : IValueFinder
        {
            var channel = new Channel(ChannelType.PointToPoint)
            {
                ConnectionStringValueFinderType = typeof(TValueFinder)
            };

            if (connectionstringprovider == null)
            {
                throw new ArgumentNullException(nameof(connectionstringprovider));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            channel.ToConnectionStringProvider = connectionstringprovider;

            channel.ToPath = path;

            _endpoint.Channels.Add(channel);

            return new AndWaitReplyFromEndPointBuilder(channel);
        }

        public void AddPublishSubscribeChannel<TValueFinder>(Func<IValueFinder, string> connectionstringprovider, string path) where TValueFinder : IValueFinder
        {
            var channel = new Channel(ChannelType.PublishSubscribe)
            {
                ConnectionStringValueFinderType = typeof(TValueFinder)
            };

            if (connectionstringprovider == null)
            {
                throw new ArgumentNullException(nameof(connectionstringprovider));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            channel.ToConnectionStringProvider = connectionstringprovider;

            channel.ToPath = path;

            _endpoint.Channels.Add(channel);
        }

        public void To(Action<IToChannelBuilder> channelbuilder)
        {
            if (channelbuilder==null)
            {
                throw new ArgumentNullException(nameof(channelbuilder));
            }

            channelbuilder(this);
        }

        public IOnEndPointOptionBuilder UseMiddleware(Action<IOutboundMiddlewareBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new OutboundMiddlewareBuilder(_endpoint);

            action(builder);

            return this;
        }

        public IOnEndPointOptionBuilder AsClaimCheck()
        {
            _endpoint.UseClaimCheck = true;

            return this;
        }

        public void To<TReply>(Action<IToReplyChannelBuilder> channelbuilder)
        {
            if (channelbuilder == null)
            {
                throw new ArgumentNullException(nameof(channelbuilder));
            }

            _endpoint.ReplyType = typeof(TReply);

            channelbuilder(this);
        }
    }
}