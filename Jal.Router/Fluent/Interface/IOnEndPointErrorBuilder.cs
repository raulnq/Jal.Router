using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Outbound;
using System.Collections.Generic;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnEndPointErrorBuilder
    {
        IForExceptionBuilder Use<TErrorMessageHandler>(IDictionary<string, object> parameters, bool stopafterhandle = true) where TErrorMessageHandler : IBusErrorMessageHandler;
    }
}