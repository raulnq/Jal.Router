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

        bool Retry<TContent>(TContent content, InboundMessageContext inboundmessagecontext, EndPointSetting endpoint, IRetryPolicy retrypolicy);

        bool Retry<TContent>(TContent content, InboundMessageContext inboundmessagecontext, EndPointSetting endpoint);

        bool Retry<TContent>(TContent content, InboundMessageContext inboundmessagecontext);
    }
}