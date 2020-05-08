using System;
using System.Collections.Generic;

namespace Jal.Router.Fluent.Interface
{
    public interface IEndpointChannelBuilder
    {
        IChannelIWhenBuilder AddPointToPointChannel(string connectionstring, string path, Type adapter = null, Type type = null, IDictionary<string, object> properties = null, string trasnportname = null);

        IChannelIWhenBuilder AddPublishSubscribeChannel(string connectionstring, string path, Type adapter = null, Type type = null, IDictionary<string, object> properties = null, string trasnportname = null);
    }
}