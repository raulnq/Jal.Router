using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IPointToPointChannel
    {
        void Send(MessageContext context);

        void Listen(Route route, Action<object>[] routeactions, string channelpath);
    }
}