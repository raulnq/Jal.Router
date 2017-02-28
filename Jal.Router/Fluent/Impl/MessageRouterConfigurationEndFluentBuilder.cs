using System;
using Jal.Factory.Model;

namespace Jal.Router.Fluent.Impl
{
    public class MessageRouterConfigurationEndFluentBuilder<TRequest>
    {
        private readonly ObjectFactoryConfigurationItem _objectFactoryConfigurationItem;

        public MessageRouterConfigurationEndFluentBuilder(ObjectFactoryConfigurationItem objectFactoryConfigurationItem)
        {
            _objectFactoryConfigurationItem = objectFactoryConfigurationItem;
        }
        public void When(Func<TRequest, bool> selector)
        {
            _objectFactoryConfigurationItem.Selector = selector;
        }
    }
}
