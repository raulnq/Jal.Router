using System;
using Jal.Router.Model;

namespace Jal.Router.Interface.Outbound
{
    public interface IBusInterceptor
    {
        void OnEntry(MessageContext context, Options options);

        void OnExit(MessageContext context, Options options);

        void OnSuccess(MessageContext context, Options options);

        void OnError(MessageContext context, Options options, Exception ex);

    }
}