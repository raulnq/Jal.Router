using System;
using Jal.Factory.Model;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Impl
{
    public class MessageRouterConfigurationFluentBuilder<TRequest>
    {
        private readonly ObjectFactoryConfigurationItem _objectFactoryConfigurationItem;

        public MessageRouterConfigurationFluentBuilder(ObjectFactoryConfigurationItem objectFactoryConfigurationItem)
        {
            _objectFactoryConfigurationItem = objectFactoryConfigurationItem;

            _objectFactoryConfigurationItem.TargetType = typeof(TRequest);
        }

        public MessageRouterConfigurationEndFluentBuilder<TRequest> To<TSubmitter>() where TSubmitter : IMessageSender<TRequest>
        {
            _objectFactoryConfigurationItem.ResultType = typeof(TSubmitter);

            return new MessageRouterConfigurationEndFluentBuilder<TRequest>(_objectFactoryConfigurationItem);
        }
    }
}
