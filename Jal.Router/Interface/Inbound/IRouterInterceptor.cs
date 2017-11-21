using System;
using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IRouterInterceptor
    {
        void OnEntry(MessageContext context);

        void OnSuccess(MessageContext context);

        void OnExit(MessageContext context);

        void OnException(MessageContext context, Exception exception);
    }
}