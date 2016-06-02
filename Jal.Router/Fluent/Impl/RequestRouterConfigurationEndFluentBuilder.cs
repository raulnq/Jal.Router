using System;
using System.Collections.Specialized;
using System.Dynamic;
using Jal.Factory.Model;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class RequestRouterConfigurationEndFluentBuilder<TRequest>
    {
        private readonly ObjectFactoryConfigurationItem _objectFactoryConfigurationItem;

        public RequestRouterConfigurationEndFluentBuilder(ObjectFactoryConfigurationItem objectFactoryConfigurationItem)
        {
            _objectFactoryConfigurationItem = objectFactoryConfigurationItem;

            _objectFactoryConfigurationItem.Bag = new ExpandoObject();
        }
        public RequestRouterConfigurationEndFluentBuilder<TRequest> When(Func<TRequest, bool> selector)
        {
            _objectFactoryConfigurationItem.Selector = selector;

            return this;
        }

        public RequestRouterConfigurationEndFluentBuilder<TRequest> With(Func<NameValueCollection, EndPoint[]> endPointProvider)
        {
            _objectFactoryConfigurationItem.Bag.EndPointProvider = endPointProvider;
            return this;
        }
    }
}
