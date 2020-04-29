using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class ChannelWhenBuilder : IChannelIWhenBuilder, IOptionBuilder
    {
        private readonly Channel _channel;

        public ChannelWhenBuilder(Channel channel)
        {
            _channel = channel;
        }

        public void ClaimCheck()
        {
            _channel.AsClaimCheck();
        }

        public void CreateIfNotExist(IDictionary<string, object> properties = null, IList<Rule> rules = null)
        {
            if(properties!=null)
            {
                foreach (var property in properties)
                {
                    _channel.Properties.Add(property.Key, property.Value);
                }
            }

            if (rules != null)
            {
                foreach (var rule in rules)
                {
                    _channel.Rules.Add(rule);
                }
            }
            _channel.CreateIfNotExists();
        }

        public void Partition(Func<MessageContext, bool> condition = null)
        {
            _channel.Partition(condition);
        }

        public IChannelOptionBuilder When(Func<MessageContext, bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            _channel.When(condition);

            return this;
        }

        public void With(Action<IOptionBuilder> action)
        {
            if (action==null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            action(this);
        }
    }
}