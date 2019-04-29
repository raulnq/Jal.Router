using System;
using Jal.Router.Model.Outbound;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IPointToPointChannel : IListenerChannel, ISenderChannel
    {

    }
}