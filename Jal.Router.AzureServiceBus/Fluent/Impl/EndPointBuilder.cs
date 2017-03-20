using System;
using Jal.Router.AzureServiceBus.Fluent.Interface;
using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.AzureServiceBus.Model;

namespace Jal.Router.AzureServiceBus.Fluent.Impl
{
    public class EndPointBuilder<TExtractor> : IToEndPointBuilder, IReplyToEndPointBuilder, IFromEndPointBuilder, INameEndPointBuilder where TExtractor : IBrokeredMessageSettingsExtractor
    {
        private readonly EndPoint _endpoint;

        public EndPointBuilder(EndPoint endpoint)
        {
            _endpoint = endpoint;

            _endpoint.ExtractorType = typeof (TExtractor);
        }

        public IFromEndPointBuilder ForMessage<TMessage>()
        {
            _endpoint.MessageType = typeof (TMessage);

            return this;
        }

        public IToEndPointBuilder From(Func<IBrokeredMessageSettingsExtractor, string> fromextractor)
        {
            _endpoint.FromExtractor = fromextractor;

            return this;
        }

        public void ReplyTo(Func<IBrokeredMessageSettingsExtractor, string> connectionstringextractor, Func<IBrokeredMessageSettingsExtractor, string> pathextractor)
        {
            _endpoint.ReplyToConnectionStringExtractor = connectionstringextractor;

            _endpoint.ReplyToPathExtractor = pathextractor;
        }

        public IReplyToEndPointBuilder To(Func<IBrokeredMessageSettingsExtractor, string> connectionstringextractor, Func<IBrokeredMessageSettingsExtractor, string> pathextractor)
        {
            _endpoint.ToConnectionStringExtractor = connectionstringextractor;

            _endpoint.ToPathExtractor = pathextractor;

            return this;
        }
    }
}