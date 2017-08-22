using System;
using Jal.Locator.Interface;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ValueSettingFinderFactory : IValueSettingFinderFactory
    {
        private readonly IServiceLocator _serviceLocator;

        public ValueSettingFinderFactory(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public IValueSettingFinder Create(Type type)
        {
            try
            {
                return _serviceLocator.Resolve<IValueSettingFinder>(type.FullName);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error during the ValueSettingFinder {nameof(IValueSettingFinder)} creation using the Type {type.FullName}", ex);
            }
        }
    }
}