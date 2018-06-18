using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IEndingListenerRouteBuilder<THandler, out TData>
    {
        IEndingNameRouteBuilder<THandler, TData> ToListen(Action<IListenerChannelBuilder> channelbuilder);
    }
}