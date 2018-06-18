using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IToEndPointBuilder
    {
        void To(Action<IToChannelBuilder> channelbuilder);
    }
}