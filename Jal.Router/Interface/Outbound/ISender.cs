using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface.Outbound
{
    public interface ISender
    {
        Task<object> Send(Channel channel, MessageContext context);
    }
}