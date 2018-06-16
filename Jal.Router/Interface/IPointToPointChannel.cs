using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IPointToPointChannel
    {
        void Send(MessageContext context);

        void Listen(Channel channel, Action<object>[] actions, string channelpath);
    }
}