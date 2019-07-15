using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class EndPointBuilder : IToEndPointBuilder, IOnEndPointOptionBuilder, INameEndPointBuilder, IToChannelBuilder, IToReplyChannelBuilder
    {
        private readonly EndPoint _endpoint;

        public EndPointBuilder(EndPoint endpoint)
        {
            _endpoint = endpoint;
        }

        public IToEndPointBuilder ForMessage<TMessage>()
        {
            _endpoint.ContentType = typeof (TMessage);

            return this;
        }

        public void AddPointToPointChannel<TValueFinder>(Func<IValueFinder, string> connectionstringprovider, string path) where TValueFinder : IValueFinder
        {
            

            if (connectionstringprovider == null)
            {
                throw new ArgumentNullException(nameof(connectionstringprovider));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var channel = new Channel(ChannelType.PointToPoint, typeof(TValueFinder), connectionstringprovider, path);

            _endpoint.Channels.Add(channel);
        }

        IAndWaitReplyFromEndPointBuilder IToReplyChannelBuilder.AddPointToPointChannel<TValueFinder>(Func<IValueFinder, string> connectionstringprovider, string path) //where TValueFinder : IValueFinder
        {
            

            if (connectionstringprovider == null)
            {
                throw new ArgumentNullException(nameof(connectionstringprovider));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            var channel = new Channel(ChannelType.PointToPoint, typeof(TValueFinder), connectionstringprovider, path);

            _endpoint.Channels.Add(channel);

            return new AndWaitReplyFromEndPointBuilder(channel);
        }

        public void AddPublishSubscribeChannel<TValueFinder>(Func<IValueFinder, string> connectionstringprovider, string path) where TValueFinder : IValueFinder
        {
            

            if (connectionstringprovider == null)
            {
                throw new ArgumentNullException(nameof(connectionstringprovider));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var channel = new Channel(ChannelType.PublishSubscribe, typeof(TValueFinder), connectionstringprovider, path);

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

        public IOnEndPointOptionBuilder To<TReply>(Action<IToReplyChannelBuilder> channelbuilder)
        {
            if (channelbuilder == null)
            {
                throw new ArgumentNullException(nameof(channelbuilder));
            }

            _endpoint.ReplyContentType = typeof(TReply);

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
    }
}