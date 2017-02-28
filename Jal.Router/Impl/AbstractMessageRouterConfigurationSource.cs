using System;
using System.Collections.Generic;
using Jal.Factory.Interface;
using Jal.Factory.Model;
using Jal.Router.Fluent.Impl;

namespace Jal.Router.Impl
{
    public abstract class AbstractMessageRouterConfigurationSource : IObjectFactoryConfigurationSource
    {
        private readonly List<ObjectFactoryConfigurationItem> _objectFactoryConfigurationItems = new List<ObjectFactoryConfigurationItem>();

        public ObjectFactoryConfiguration Source()
        {
            var result = new ObjectFactoryConfiguration();

            foreach (var item in _objectFactoryConfigurationItems)
            {
                result.Items.Add(item);
            }

            return result;
        }

        public MessageRouterConfigurationFluentBuilder<TMessage> Route<TMessage>()
        {
            var value = new ObjectFactoryConfigurationItem(typeof(TMessage));

            var descriptor = new MessageRouterConfigurationFluentBuilder<TMessage>(value);
            
            _objectFactoryConfigurationItems.Add(value);

            return descriptor;
        }

        public void Route<TMessage>(string name, Action<MessageRouterConfigurationGroupFluentBuilder<TMessage>> action)
        {
            var descriptor = new MessageRouterConfigurationGroupFluentBuilder<TMessage>(_objectFactoryConfigurationItems, name);

            action(descriptor);
        }
    }     
}
