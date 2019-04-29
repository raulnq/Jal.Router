using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IMessageRouter
    {
        Task Route(MessageContext context);

        Task Route(MessageContext context, object data);
    }
}