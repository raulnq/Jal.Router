using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IChannelPathBuilder
    {
        string BuildFromRoute(string name, Channel channel);

        string BuildFromSagaAndRoute(Saga saga, string name, Channel channel);

        string BuildFromEndpoint(string name, Channel channel);

        string BuildReplyFromEndpoint(string name, Channel channel);
    }
}