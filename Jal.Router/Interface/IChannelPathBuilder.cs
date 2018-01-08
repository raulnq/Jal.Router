using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IChannelPathBuilder
    {
        string Build(Route route);

        string Build(Saga saga, Route route);

        string Build(MessageContext context);
    }
}