using System;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Interface
{
    public interface IPublishSubscribeChannel
    {
        void Send(Channel channel, MessageContext context, string channelpath);

        void Listen(ListenerMetadata metadata);
    }
}