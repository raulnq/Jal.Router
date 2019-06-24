using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class OnRouteEntryBuilder : IOnRouteEntryBuilder
    {
        private readonly Route _route;
        public OnRouteEntryBuilder(Route route)
        {
            _route = route;
        }

        public void Use<TMessageHandler>(IDictionary<string, object> parameters) where TMessageHandler : IRouteEntryMessageHandler
        {
            var handler = new Handler(typeof(TMessageHandler), parameters);

            _route.EntryHandlers.Add(handler);
        }
    }
}