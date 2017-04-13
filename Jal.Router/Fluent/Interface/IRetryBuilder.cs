using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IRetryBuilder
    {
        IRetryUsingBuilder Retry<TExeption>() where TExeption : Exception;
    }
}