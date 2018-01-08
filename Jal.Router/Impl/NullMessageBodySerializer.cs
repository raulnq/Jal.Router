using System;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class NullMessageBodySerializer : IMessageBodySerializer
    {
        public TContent Deserialize<TContent>(string content)
        {
            return default(TContent);
        }

        public object Deserialize(string content, Type type)
        {
            return null;
        }

        public string Serialize<TContent>(TContent content)
        {
            return string.Empty;
        }

        public string Serialize(object content)
        {
            return string.Empty;
        }
    }
}