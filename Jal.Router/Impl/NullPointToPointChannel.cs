using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class NullPointToPointChannel : IPointToPointChannel
    {
        public void Send(Channel channel, MessageContext context, string channelpath)
        {
            
        }

        public void Listen(Channel channel, Action<object>[] actions, string channelpath)
        {

        }
    }
}