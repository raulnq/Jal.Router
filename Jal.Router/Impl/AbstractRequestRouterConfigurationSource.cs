using System;
using System.Collections.Generic;
using Jal.Factory.Interface;
using Jal.Factory.Model;
using Jal.Router.Fluent.Impl;

namespace Jal.Router.Impl
{
    public abstract class AbstractRequestRouterConfigurationSource : IObjectFactoryConfigurationSource
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

        public RequestRouterConfigurationFluentBuilder<TResquest, TResponse> Route<TResquest, TResponse>()
        {
            var value = new ObjectFactoryConfigurationItem(typeof(TResquest));

            var descriptor = new RequestRouterConfigurationFluentBuilder<TResquest, TResponse>(value);
            
            _objectFactoryConfigurationItems.Add(value);

            return descriptor;
        }

        public void Route<TRequest, TResponse>(string name, Action<RequestRouterConfigurationGroupFluentBuilder<TRequest, TResponse>> action)
        {
            var descriptor = new RequestRouterConfigurationGroupFluentBuilder<TRequest, TResponse>(_objectFactoryConfigurationItems, name);

            action(descriptor);
        }
    }     
}
