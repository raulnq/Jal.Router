using Jal.Router.Interface;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class NullMessageStorage : IMessageStorage
    {
        public Task<string> Read(string id)
        {
            return Task.FromResult(string.Empty);
        }

        public void Write(string id, string content)
        {
            
        }
    }
}