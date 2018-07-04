using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class EndPointBuilder : IOnEndPointOptionBuilder, INameEndPointBuilder, IToChannelBuilder
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

        public IAndWaitReplyFromEndPointBuilder Add<TExtractorConnectionString>(Func<IValueSettingFinder, string> connectionstringextractor, string path) where TExtractorConnectionString : IValueSettingFinder
        {
            var channel = new Channel
            {
                ConnectionStringExtractorType = typeof(TExtractorConnectionString)
            };

            if (connectionstringextractor == null)
            {
                throw new ArgumentNullException(nameof(connectionstringextractor));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            channel.ToConnectionStringExtractor = connectionstringextractor;

            channel.ToPath = path;

            _endpoint.Channels.Add(channel);

            return new AndWaitReplyFromEndPointBuilder(channel);
        }

        public void To(Action<IToChannelBuilder> channelbuilder)
        {
            if (channelbuilder==null)
            {
                throw new ArgumentNullException(nameof(channelbuilder));
            }

            channelbuilder(this);
        }

        public IOnEndPointOptionBuilder UsingMiddleware(Action<IOutboundMiddlewareBuilder> action)
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
    }
}