using System;
using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IMessageAdapter
    {
        MessageContext Read(object message, Type contenttype);

        string GetBody(object message);

        object Write(MessageContext context);
    }
}