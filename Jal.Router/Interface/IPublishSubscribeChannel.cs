using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IPublishSubscribeChannel
    {
        void Send(MessageContext context);

        void Listen(Channel channel, Action<object>[] routeactions, string channelpath);
    }
}