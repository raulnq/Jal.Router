using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IPublishSubscribeChannel
    {
        void Send<TContent>(MessageContext<TContent> context);

        void Listen(string connectionstring, string path, string subscription, Saga saga, Route route, bool startingroute);
    }
}