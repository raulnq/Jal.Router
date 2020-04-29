using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IWhenRouteBuilder: IOnRouteOptionBuilder
    {
        IOnRouteOptionBuilder When(Func<MessageContext, bool> condition);
    }
}