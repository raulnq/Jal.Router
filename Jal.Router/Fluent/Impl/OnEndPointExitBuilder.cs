using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class OnEndPointExitBuilder : IOnEndPointExitBuilder
    {
        private readonly EndPoint _endpoint;
        public OnEndPointExitBuilder(EndPoint endpoint)
        {
            _endpoint = endpoint;
        }

        public void Use<TMessageHandler>(IDictionary<string, object> parameters) where TMessageHandler : IBusExitMessageHandler
        {
            var handler = new Handler(typeof(TMessageHandler), parameters);

            _endpoint.EntryHandlers.Add(handler);
        }
    }
}