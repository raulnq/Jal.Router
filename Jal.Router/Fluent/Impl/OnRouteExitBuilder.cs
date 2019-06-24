using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class OnRouteExitBuilder : IOnRouteExitBuilder
    {
        private readonly Route _route;
        public OnRouteExitBuilder(Route route)
        {
            _route = route;
        }

        public void Use<TMessageHandler>(IDictionary<string, object> parameters) where TMessageHandler : IRouteExitMessageHandler
        {
            var handler = new Handler(typeof(TMessageHandler), parameters);

            _route.EntryHandlers.Add(handler);
        }
    }
}