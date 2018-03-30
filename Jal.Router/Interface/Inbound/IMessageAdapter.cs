using System;
using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IMessageAdapter
    {
        MessageContext Read(object message, Type contenttype);

        object Write(MessageContext context);

        object Deserialize(string content, Type type);

        TContent Deserialize<TContent>(string content);

        string Serialize(object content);
    }
}