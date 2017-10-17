using Jal.Router.Interface;
using Jal.Router.Model.Outbount;

namespace Jal.Router.Impl
{
    public class NullPublishSubscribeChannel : IPublishSubscribeChannel
    {
        public void Send<TContent>(OutboundMessageContext<TContent> context)
        {
            
        }
    }
}