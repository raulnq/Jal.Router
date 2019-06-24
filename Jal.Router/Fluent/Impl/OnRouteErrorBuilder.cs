using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class OnRouteErrorBuilder : IOnRouteErrorBuilder
    {
        private readonly Route _route;

        public OnRouteErrorBuilder(Route route)
        {
            _route = route;
        }

        public IForExceptionBuilder Use<TErrorMessageHandler>(IDictionary<string, object> parameters, bool stopafterhandle = true) where TErrorMessageHandler : IRouteErrorMessageHandler
        {
            var handler = new ErrorHandler(typeof(TErrorMessageHandler), parameters, stopafterhandle);

            _route.ErrorHandlers.Add(handler);

            return new ForExceptionBuilder(handler.ExceptionTypes);
        }
    }
}