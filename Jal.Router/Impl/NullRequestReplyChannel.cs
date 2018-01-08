using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class NullRequestReplyChannel : IRequestReplyChannel
    {
        public object Reply(MessageContext context, Type resulttype)
        {
            return null;
        }
    }
}