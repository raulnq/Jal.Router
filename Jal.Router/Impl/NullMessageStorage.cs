using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class NullMessageStorage : IMessageStorage
    {
        public string Read(string id)
        {
            return string.Empty;
        }

        public void Write(string id, string content)
        {
            
        }
    }
}