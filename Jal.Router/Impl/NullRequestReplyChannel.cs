using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class NullRequestReplyChannel : IRequestReplyChannel
    {
        public object Reply(Channel channel, MessageContext context, string channelpath)
        {
            return null;
        }
    }
}