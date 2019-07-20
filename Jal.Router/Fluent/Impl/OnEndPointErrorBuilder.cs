using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class OnEndPointErrorBuilder : IOnEndPointErrorBuilder
    {
        private readonly EndPoint _endpoint;

        public OnEndPointErrorBuilder(EndPoint endpoint)
        {
            _endpoint = endpoint;
        }

        public IForExceptionBuilder Use<TErrorMessageHandler>(IDictionary<string, object> parameters, bool stopafterhandle = true) where TErrorMessageHandler : IBusErrorMessageHandler
        {
            var handler = new ErrorHandler(typeof(TErrorMessageHandler), parameters, stopafterhandle);

            _endpoint.ErrorHandlers.Add(handler);

            return new ForExceptionBuilder(handler.ExceptionTypes);
        }
    }
}