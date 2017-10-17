using System;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Interface.Inbound
{
    public interface IRouterLogger
    {
        void OnEntry(MessageContext context);

        void OnSuccess<TContent>(MessageContext context, TContent content);

        void OnExit(MessageContext context, long duration);

        void OnException(MessageContext context, Exception exception);
    }
}