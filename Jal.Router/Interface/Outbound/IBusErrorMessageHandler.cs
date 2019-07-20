using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IBusErrorMessageHandler
    {
        Task<bool> OnException(MessageContext context, Exception ex, ErrorHandler metadata);
    }
}