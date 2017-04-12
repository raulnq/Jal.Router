using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IBus
    {
        void Send<TContent>(TContent content, Options options);

        void Send<TContent>(TContent content, Origin origin, Options options);

        void Send<TContent>(TContent content, EndPointSetting endpoint, Origin origin, Options options);

        void FireAndForget<TContent>(TContent content, Options options);

        void FireAndForget<TContent>(TContent content, EndPointSetting endpoint, Origin origin, Options options);

        void FireAndForget<TContent>(TContent content, Origin origin, Options options);

        void Publish<TContent>(TContent content, Options options);

        void Publish<TContent>(TContent content, Origin origin, Options options);

        void Publish<TContent>(TContent content, EndPointSetting endpoint, Origin origin, Options options);

        void Retry<TContent>(TContent content, InboundMessageContext inboundmessagecontext, EndPointSetting endpoint, IRetryPolicy retrypolicy);

        void Retry<TContent>(TContent content, InboundMessageContext inboundmessagecontext, EndPointSetting endpoint);

        void Retry<TContent>(TContent content, InboundMessageContext inboundmessagecontext);

        bool CanRetry<TContent>(TContent content, InboundMessageContext inboundmessagecontext, IRetryPolicy retrypolicy);

        bool CanRetry<TContent>(TContent content, InboundMessageContext inboundmessagecontext);
    }
}