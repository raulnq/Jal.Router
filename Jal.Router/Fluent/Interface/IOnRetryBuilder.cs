using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnRetryBuilder : IOnErrorBuilder
    {
        IOnRetryUsingBuilder OnExceptionRetryFailedMessageTo<TExeption>(string endpointname) where TExeption : Exception;
    }
}