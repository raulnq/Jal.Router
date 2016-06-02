using System.Collections.Generic;
using Jal.Factory.Model;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Impl
{
    public class RequestRouterConfigurationGroupFluentBuilder<TRequest, TResponse>
    {
        private readonly List<ObjectFactoryConfigurationItem> _objectFactoryConfigurationItems;

        private readonly string _groupName;

        public RequestRouterConfigurationGroupFluentBuilder(List<ObjectFactoryConfigurationItem> objectFactoryConfigurationItems, string groupName)
        {
            _objectFactoryConfigurationItems = objectFactoryConfigurationItems;

            _groupName = groupName;
        }

        public RequestRouterConfigurationEndFluentBuilder<TRequest> To<TSubmitter>() where TSubmitter : IRequestSender<TRequest, TResponse>
        {

            var value = new ObjectFactoryConfigurationItem(typeof(TRequest)) { ResultType = typeof(TSubmitter), GroupName = _groupName };

            _objectFactoryConfigurationItems.Add(value);

            return new RequestRouterConfigurationEndFluentBuilder<TRequest>(value);
        }
    }
}