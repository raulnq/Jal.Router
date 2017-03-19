using System;

namespace Jal.Router.Interface
{
    public interface IConsumerFactory
    {
        T Create<T>(Type consumertype) where T : class;
    }
}