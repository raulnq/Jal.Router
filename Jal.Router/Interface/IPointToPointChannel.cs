using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IPointToPointChannel
    {
        void Send<TContent>(MessageContext<TContent> context);

        void Listen(string connectionstring, string path, Saga saga, Route route, bool startingroute);
    }
}