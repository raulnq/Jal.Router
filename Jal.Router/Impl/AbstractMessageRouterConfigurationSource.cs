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

        public MessageRouterConfigurationFluentBuilder<TResquest> Route<TResquest>()
        {
            var value = new ObjectFactoryConfigurationItem(typeof(TResquest));

            var descriptor = new MessageRouterConfigurationFluentBuilder<TResquest>(value);
            
            _objectFactoryConfigurationItems.Add(value);

            return descriptor;
        }

        public void Route<TRequest>(string name, Action<MessageRouterConfigurationGroupFluentBuilder<TRequest>> action)
        {
            var descriptor = new MessageRouterConfigurationGroupFluentBuilder<TRequest>(_objectFactoryConfigurationItems, name);

            action(descriptor);
        }
    }     
}
