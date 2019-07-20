using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface ISender
    {
        Task Send(MessageContext context);
    }
}