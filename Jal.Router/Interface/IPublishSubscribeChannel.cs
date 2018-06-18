using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IPublishSubscribeChannel
    {
        void Send(Channel channel, MessageContext context, string channelpath);

        void Listen(Channel channel, Action<object>[] actions, string channelpath);
    }
}