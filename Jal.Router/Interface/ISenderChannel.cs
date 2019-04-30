using Jal.Router.Model.Outbound;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface ISenderChannel
    {
        void Open(SenderMetadata metadata);

        Task<string> Send(object message);

        Task Close();
    }
}