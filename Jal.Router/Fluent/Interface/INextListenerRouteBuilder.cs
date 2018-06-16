using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface INextListenerRouteBuilder<THandler, out TData>
    {
        INextNameRouteBuilder<THandler, TData> ToListen(Action<IListenerChannelBuilder> channelbuilder);
    }
}