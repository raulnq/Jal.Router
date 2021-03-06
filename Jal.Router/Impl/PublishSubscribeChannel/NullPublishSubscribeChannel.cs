﻿using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class NullPublishSubscribeChannel : IPublishSubscribeChannel
    {
        public Task<Statistic> GetStatistic(Channel channel)
        {
            return Task.FromResult(default(Statistic));
        }

        public Task<bool> DeleteIfExist(Channel channel)
        {
            return Task.FromResult(false);
        }

        public Task<MessageContext> Read(SenderContext sendercontext, MessageContext context, IMessageAdapter adapter)
        {
            return Task.FromResult(default(MessageContext));
        }

        public void Open(SenderContext sendercontext)
        {

        }

        public Task<string> Send(SenderContext sendercontext, object message)
        {
            return Task.FromResult(string.Empty);
        }

        public bool IsActive(SenderContext sendercontext)
        {
            return false;
        }

        public Task Close(SenderContext sendercontext)
        {
            return Task.CompletedTask;
        }

        public Task<bool> CreateIfNotExist(Channel channel)
        {
            return Task.FromResult(false);
        }
    }
}