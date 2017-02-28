using System;
using Jal.Factory.Model;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Impl
{
    public class MessageRouterConfigurationFluentBuilder<TMessage>
    {
        private readonly ObjectFactoryConfigurationItem _objectFactoryConfigurationItem;

        public MessageRouterConfigurationFluentBuilder(ObjectFactoryConfigurationItem objectFactoryConfigurationItem)
        {
            _objectFactoryConfigurationItem = objectFactoryConfigurationItem;

            _objectFactoryConfigurationItem.TargetType = typeof(TMessage);
        }

        public MessageRouterConfigurationEndFluentBuilder<TMessage> To<THandler>() where THandler : IMessageHandler<TMessage>
        {
            _objectFactoryConfigurationItem.ResultType = typeof(THandler);

            return new MessageRouterConfigurationEndFluentBuilder<TMessage>(_objectFactoryConfigurationItem);
        }
    }
}
