using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IBus
    {
        void Reply<TContent>(TContent content, InboundMessageContext context);//TODO delete

        void Send<TContent>(TContent content, Options options);

        void Send<TContent>(TContent content, EndPointSetting endpoint, Options options);

        void FireAndForget<TContent>(TContent content, Options options);

        void FireAndForget<TContent>(TContent content, EndPointSetting endpoint, Options options);

        void Publish<TContent>(TContent content, Options options);

        void Publish<TContent>(TContent content, EndPointSetting endpoint, Options options);
    }
}