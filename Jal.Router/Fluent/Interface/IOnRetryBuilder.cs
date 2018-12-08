using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnRetryBuilder : IOnRouteOptionBuilder
    {
        IOnRetryUsingBuilder OnExceptionRetryFailedMessageTo(string endpointname, Action<IForExceptionBuilder> action);
    }
}