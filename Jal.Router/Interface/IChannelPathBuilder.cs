using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IChannelPathBuilder
    {
        string BuildFromRoute(Route route);

        string BuildFromSagaAndRoute(Saga saga, Route route);

        string BuildFromContext(MessageContext context);

        string BuildReplyFromContext(MessageContext context);
    }
}