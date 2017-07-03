using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IWhenHandlerBuilder<out TContent>
    {
        void When(Func<TContent, InboundMessageContext, bool> condition);
    }
}