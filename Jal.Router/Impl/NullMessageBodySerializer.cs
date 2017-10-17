using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class NullMessageBodySerializer : IMessageBodySerializer
    {
        public TContent Deserialize<TContent>(string body)
        {
            return default(TContent);
        }

        public string Serialize<TContent>(TContent body)
        {
            return string.Empty;
        }
    }
}