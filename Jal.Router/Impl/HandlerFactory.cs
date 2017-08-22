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

        public THandler Create<THandler>(Type type) where THandler : class
        {
            try
            {
                return _serviceLocator.Resolve<THandler>(type.FullName);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the Handler {nameof(THandler)} creation using the Type {type.FullName}", ex);
            }
        }
    }
}
