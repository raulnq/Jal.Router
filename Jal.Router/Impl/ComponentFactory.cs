using System;
using Jal.Locator;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ComponentFactory : IComponentFactory
    {
        private readonly IServiceLocator _serviceLocator;

        public ComponentFactory(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public TComponent Create<TComponent>(Type type) where TComponent : class
        {
            try
            {
                return _serviceLocator.Resolve<TComponent>(type.FullName);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the component {typeof(TComponent).FullName} creation using the Type {type.FullName}", ex);
            }
        }
    }
}