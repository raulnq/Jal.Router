using System;
using Jal.Locator.Interface;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ConsumerFactory : IConsumerFactory
    {
        private readonly IServiceLocator _serviceLocator;

        public ConsumerFactory(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public T Create<T>(Type consumertype) where T : class
        {
            return _serviceLocator.Resolve<T>(consumertype.FullName);
        }
    }
}
