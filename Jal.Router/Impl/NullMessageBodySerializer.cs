using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class NullMessageBodySerializer : IMessageBodySerializer
    {
        public TContent Deserialize<TContent>(string content)
        {
            return default(TContent);
        }

        public string Serialize<TContent>(TContent content)
        {
            return string.Empty;
        }
    }
}