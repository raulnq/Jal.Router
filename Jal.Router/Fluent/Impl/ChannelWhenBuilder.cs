using System;
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

        public void Partition(Func<MessageContext, bool> condition = null)
        {
            _channel.UsePartitions(condition);
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