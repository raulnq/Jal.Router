using Jal.Router.Model;
using System;
using System.Threading.Tasks;

namespace Jal.Router.Interface.Inbound
{
    public interface IRouteErrorMessageHandler
    {
        Task<bool> OnException(MessageContext context, Exception ex, ErrorHandler metadata);
    }
}