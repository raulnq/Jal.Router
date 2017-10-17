using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnRetryBuilder : IOnOptionBuilder
    {
        IOnRetryUsingBuilder OnExceptionRetryFailedMessageTo<TExeption>(string endpointname) where TExeption : Exception;
    }
}