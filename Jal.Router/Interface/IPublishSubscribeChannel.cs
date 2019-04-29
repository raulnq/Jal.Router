using System;
using System.Threading.Tasks;
using Jal.Router.Model.Inbound;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Interface
{
    public interface IPublishSubscribeChannel : IListenerChannel, ISenderChannel
    {

    }
}