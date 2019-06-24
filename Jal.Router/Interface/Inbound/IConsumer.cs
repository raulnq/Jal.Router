using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IConsumer
    {
        Task Consume(MessageContext context);
    }
}