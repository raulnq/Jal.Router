using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IBus
    {
        void ReplyTo<TContent>(TContent content, InboundMessageContext context);

        void Send<TContent>(TContent content, InboundMessageContext context, string id="", string name = "");

        void Send<TContent>(TContent content, InboundMessageContext context, EndPointSetting endpoint, string id="");
    }
}