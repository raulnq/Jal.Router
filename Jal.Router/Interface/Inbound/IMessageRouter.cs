using System;
using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IMessageRouter
    {
        void Route(MessageContext context, Route route);

        void Route(MessageContext context, Route route, object data, Type datatype);
    }
}