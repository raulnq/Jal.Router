using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnRetryBuilder : IOnRouteOptionBuilder
    {
        IOnRetryUsingBuilder OnExceptionRetryFailedMessageTo<TExeption>(string endpointname) where TExeption : Exception;
    }
}