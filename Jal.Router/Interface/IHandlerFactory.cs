using System;

namespace Jal.Router.Interface
{
    public interface IHandlerFactory
    {
        THandler Create<THandler>(Type type) where THandler : class;
    }
}