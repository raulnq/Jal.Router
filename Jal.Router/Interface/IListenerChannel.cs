using Jal.Router.Model.Inbound;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IListenerChannel
    {
        void Open(ListenerMetadata metadata);

        bool IsActive();

        Task Close();

        void Listen();
    }
}