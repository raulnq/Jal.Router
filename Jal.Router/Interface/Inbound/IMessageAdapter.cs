using System;
using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IMessageAdapter
    {
        MessageContext Read(object message, Type contenttype, bool useclaimcheck, Identity identityconfiguration=null);

        object Write(MessageContext context, bool useclaimcheck);

        object Deserialize(string content, Type contenttype);

        TContent Deserialize<TContent>(string content);

        string Serialize(object content);
    }
}