using System.Collections.Generic;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Outbound;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnEndPointExitBuilder
    {
        void Use<TMessageHandler>(IDictionary<string, object> parameters) where TMessageHandler : IBusExitMessageHandler;
    }
}