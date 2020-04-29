using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IReplyIEndpointChannelBuilder
    {
        IAndWaitReplyFromBuilder AddPointToPointChannel(string connectionstring, string path);

    }
}