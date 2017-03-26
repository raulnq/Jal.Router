using System;
using Jal.Locator.Interface;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class EndPointSettingFinderFactory : IEndPointSettingFinderFactory
    {
        private readonly IServiceLocator _serviceLocator;

        public EndPointSettingFinderFactory(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public IEndPointValueSettingFinder Create(Type type)
        {
            return _serviceLocator.Resolve<IEndPointValueSettingFinder>(type.FullName);
        }

        public IEndPointSettingFinder<T> Create<T>(Type type)
        {
            return _serviceLocator.Resolve<IEndPointSettingFinder<T>>(type.FullName);
        }
    }
}