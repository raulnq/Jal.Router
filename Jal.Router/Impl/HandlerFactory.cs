using System;
using Jal.Locator.Interface;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class HandlerFactory : IHandlerFactory
    {
        private readonly IServiceLocator _serviceLocator;

        public HandlerFactory(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public T Create<T>(Type type) where T : class
        {
            return _serviceLocator.Resolve<T>(type.FullName);
        }
    }
}
