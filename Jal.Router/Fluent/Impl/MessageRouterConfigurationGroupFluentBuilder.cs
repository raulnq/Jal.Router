using System.Collections.Generic;
using Jal.Factory.Model;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Impl
{
    public class MessageRouterConfigurationGroupFluentBuilder<TRequest>
    {
        private readonly List<ObjectFactoryConfigurationItem> _objectFactoryConfigurationItems;

        private readonly string _name;

        public MessageRouterConfigurationGroupFluentBuilder(List<ObjectFactoryConfigurationItem> objectFactoryConfigurationItems, string name)
        {
            _objectFactoryConfigurationItems = objectFactoryConfigurationItems;

            _name = name;
        }

        public MessageRouterConfigurationEndFluentBuilder<TRequest> To<TSubmitter>() where TSubmitter : IMessageSender<TRequest>
        {

            var value = new ObjectFactoryConfigurationItem(typeof(TRequest)) { ResultType = typeof(TSubmitter), Name = _name };

            _objectFactoryConfigurationItems.Add(value);

            return new MessageRouterConfigurationEndFluentBuilder<TRequest>(value);
        }
    }
}