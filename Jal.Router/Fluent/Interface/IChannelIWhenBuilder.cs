using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IChannelIWhenBuilder : IChannelOptionBuilder
    {
        IChannelOptionBuilder When(Func<MessageContext, bool> condition);
    }
}