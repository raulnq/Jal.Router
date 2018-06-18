using System;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class NullMessageAdapter : IMessageAdapter
    {
        public MessageContext Read(object message, Type contenttype)
        {
            return new MessageContext(new EndPoint(string.Empty));
        }

        public object Write(MessageContext context)
        {
            return null;
        }

        public object Deserialize(string content, Type type)
        {
            return null;
        }

        public TContent Deserialize<TContent>(string content)
        {
            return default(TContent);
        }

        public string Serialize(object content)
        {
            return string.Empty;
        }
    }
}