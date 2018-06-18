using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class NullPublishSubscribeChannel : IPublishSubscribeChannel
    {
        public void Send(Channel channel, MessageContext context, string channelpath)
        {
            
        }

        public void Listen(Channel channel, Action<object>[] routeactions, string channelpath)
        {

        }
    }
}