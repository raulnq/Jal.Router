using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class NullPublishSubscribeChannel : IPublishSubscribeChannel
    {
        public void Send<TContent>(MessageContext<TContent> context)
        {
            
        }

        public void Listen(string connectionstring, string path, string subscription, Saga saga, Route route, bool startingroute)
        {
            
        }
    }
}