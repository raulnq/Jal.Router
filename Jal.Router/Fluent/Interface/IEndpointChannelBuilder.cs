using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IEndpointChannelBuilder
    {
        IChannelIWhenBuilder AddPointToPointChannel(string connectionstring, string path, Type adapter = null, Type type = null);

        IChannelIWhenBuilder AddPublishSubscribeChannel(string connectionstring, string path, Type adapter = null, Type type = null);
    }
}