using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IBus
    {
        void Reply<TContent>(TContent content, InboundMessageContext context);

        void Send<TContent>(TContent content, Options options);

        void Send<TContent>(TContent content, EndPointSetting endpoint, Options options);
    }
}