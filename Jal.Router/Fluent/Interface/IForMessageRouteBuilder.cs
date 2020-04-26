using Jal.Router.Model;
using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IForMessageRouteBuilder
    {
        IUseMethodBuilder<TContent> ForMessage<TContent>(Func<MessageContext, bool> condition=null);
    }

    public interface IForMessageRouteBuilder<out TData>
    {
        IUseMethodBuilder<TContent, TData> ForMessage<TContent>(Func<MessageContext, bool> condition = null);
    }
}