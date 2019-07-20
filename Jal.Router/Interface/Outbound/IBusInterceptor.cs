using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IBusInterceptor
    {
        void OnEntry(MessageContext context);

        void OnExit(MessageContext context);

        void OnSuccess(MessageContext context);

        void OnError(MessageContext context, Exception ex);

    }
}