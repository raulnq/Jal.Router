using System;
using Jal.Router.Interface;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl
{
    public class NullPointToPointChannel : IPointToPointChannel
    {
        public void Send(Channel channel, MessageContext context, string channelpath)
        {
            
        }

        public void Listen(ListenerMetadata metadata)
        {

        }
    }
}