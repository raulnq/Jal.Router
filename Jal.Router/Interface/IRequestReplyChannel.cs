using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRequestReplyChannel
    {
        object Reply(Channel channel, MessageContext context, string channelpath);
    }
}