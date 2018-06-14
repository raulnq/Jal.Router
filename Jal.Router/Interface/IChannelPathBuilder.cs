using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IChannelPathBuilder
    {
        string BuildFromRoute(string routeName, Channel channel);

        string BuildFromSagaAndRoute(Saga saga, string routeName, Channel channel);

        string BuildFromContext(MessageContext context);

        string BuildReplyFromContext(MessageContext context);
    }
}