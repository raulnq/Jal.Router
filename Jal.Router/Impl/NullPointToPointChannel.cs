using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class NullPointToPointChannel : IPointToPointChannel
    {
        public void Send(MessageContext context)
        {
            
        }

        public void Listen(Channel channel, Action<object>[] actions, string channelpath)
        {

        }
    }
}