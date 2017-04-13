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
            return _serviceLocator.Resolve<IValueSettingFinder>(type.FullName);
        }
    }
}