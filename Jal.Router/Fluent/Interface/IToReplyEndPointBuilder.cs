using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IToReplyEndPointBuilder
    {
        void To<TReply>(Action<IToReplyChannelBuilder> channelbuilder);
    }
}