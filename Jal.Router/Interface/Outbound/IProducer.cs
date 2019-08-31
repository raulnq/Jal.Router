using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IProducer
    {
        Task Send(MessageContext context);
    }
}