using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IConsumer
    {
        Task Consume(MessageContext context);
    }
}