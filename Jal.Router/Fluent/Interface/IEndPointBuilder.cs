using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IEndPointBuilder
    {
        IWhenEndpointBuilder To(Action<IEndpointChannelBuilder> channelbuilder);

        IWhenEndpointBuilder To<TReply>(Action<IReplyIEndpointChannelBuilder> channelbuilder);
    }
}