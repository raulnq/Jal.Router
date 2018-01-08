using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRequestReplyChannel
    {
        object Reply(MessageContext context, Type resulttype);
    }
}