using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IToEndPointBuilder
    {
        IOnEndPointOptionBuilder To(Action<IToChannelBuilder> channelbuilder);

        IOnEndPointOptionBuilder To<TReply>(Action<IToReplyChannelBuilder> channelbuilder);
    }
}