using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IWhenHandlerBuilder: IOnRouteOptionBuilder
    {
        IOnRouteOptionBuilder When(Func<MessageContext, bool> condition);
    }
}