using System;
using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IMessageRouter
    {
        void Route(MessageContext context);

        void Route(MessageContext context, object data);
    }
}