using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IMessageStorage
    {
        Task<string> Read(string id);
        void Write(string id, string content);
    }
}