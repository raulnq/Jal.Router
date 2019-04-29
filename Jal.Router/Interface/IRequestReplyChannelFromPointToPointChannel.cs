using System;
using System.Threading.Tasks;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Interface
{
    public interface IRequestReplyChannelFromPointToPointChannel : ISenderChannel, IReaderChannel
    {

    }
}