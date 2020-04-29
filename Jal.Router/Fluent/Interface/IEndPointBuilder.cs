using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IEndPointBuilder
    {
        IOnEndPointOptionBuilder To(Action<IEndpointChannelBuilder> channelbuilder);

        IOnEndPointOptionBuilder To<TReply>(Action<IReplyIEndpointChannelBuilder> channelbuilder);
    }
}