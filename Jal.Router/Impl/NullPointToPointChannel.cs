using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class NullPointToPointChannel : IPointToPointChannel
    {
        public void Send<TContent>(MessageContext<TContent> context)
        {

        }

        public void Listen(string connectionstring, string path, Saga saga, Route route, bool startingroute)
        {
            
        }
    }
}