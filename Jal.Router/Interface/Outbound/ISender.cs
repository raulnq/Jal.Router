using Jal.Router.Model;

namespace Jal.Router.Interface.Outbound
{
    public interface ISender
    {
        object Send(Channel channel, MessageContext context);
    }
}