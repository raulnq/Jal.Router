using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IWhenHandlerBuilder<out TContent> : IOnRetryBuilder
    {
        IOnRetryBuilder When(Func<TContent, MessageContext, bool> condition);
    }
}