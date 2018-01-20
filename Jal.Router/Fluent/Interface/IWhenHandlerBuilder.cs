using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IWhenHandlerBuilder: IOnRetryBuilder
    {
        IOnRetryBuilder When(Func<MessageContext, bool> condition);
    }
}