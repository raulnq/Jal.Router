using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IForExceptionBuilder
    {
        void For<TException>() where TException : Exception;

        void For<TExceptionA, TExceptionB>() where TExceptionA : Exception  where TExceptionB : Exception;
    }
}