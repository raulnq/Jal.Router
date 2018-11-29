using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IForExceptionBuilder
    {
        void For<TExeption>() where TExeption : Exception;
    }
}