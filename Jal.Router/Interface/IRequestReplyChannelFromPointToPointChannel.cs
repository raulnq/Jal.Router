using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRequestReplyChannelFromPointToPointChannel : ISenderChannel, IReaderChannel
    {

    }
}