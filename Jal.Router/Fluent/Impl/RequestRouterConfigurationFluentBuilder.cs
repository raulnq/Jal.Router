using System;
using Jal.Factory.Model;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Impl
{
    public class RequestRouterConfigurationFluentBuilder<TRequest, TResponse>
    {
        private readonly ObjectFactoryConfigurationItem _objectFactoryConfigurationItem;

        public RequestRouterConfigurationFluentBuilder(ObjectFactoryConfigurationItem objectFactoryConfigurationItem)
        {
            _objectFactoryConfigurationItem = objectFactoryConfigurationItem;

            _objectFactoryConfigurationItem.TargetType = typeof(TRequest);
        }

        public RequestRouterConfigurationEndFluentBuilder<TRequest> To<TSubmitter>() where TSubmitter : IRequestSender<TRequest, TResponse>
        {
            _objectFactoryConfigurationItem.ResultType = typeof(TSubmitter);

            return new RequestRouterConfigurationEndFluentBuilder<TRequest>(_objectFactoryConfigurationItem);
        }
    }
}
