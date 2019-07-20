using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IMessageStorage
    {
        Task<string> Read(string id);
        Task Write(string id, string content);
    }
}