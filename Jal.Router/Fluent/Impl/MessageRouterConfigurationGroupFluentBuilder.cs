using System.Collections.Generic;
using Jal.Factory.Model;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Impl
{
    public class MessageRouterConfigurationGroupFluentBuilder<TMessage>
    {
        private readonly List<ObjectFactoryConfigurationItem> _objectFactoryConfigurationItems;

        private readonly string _name;

        public MessageRouterConfigurationGroupFluentBuilder(List<ObjectFactoryConfigurationItem> objectFactoryConfigurationItems, string name)
        {
            _objectFactoryConfigurationItems = objectFactoryConfigurationItems;

            _name = name;
        }

        public MessageRouterConfigurationEndFluentBuilder<TMessage> To<THandler>() where THandler : IMessageHandler<TMessage>
        {

            var value = new ObjectFactoryConfigurationItem(typeof(TMessage)) { ResultType = typeof(THandler), Name = _name };

            _objectFactoryConfigurationItems.Add(value);

            return new MessageRouterConfigurationEndFluentBuilder<TMessage>(value);
        }
    }
}