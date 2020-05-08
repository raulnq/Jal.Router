using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using System;
using System.Threading.Tasks;

namespace Jal.Router.SqlServer.Impl
{
    public class SqlServerTopic : AbstractChannel, IPublishSubscribeChannel
    {
        public SqlServerTopic(IComponentFactoryFacade factory, ILogger logger) : base(factory, logger)
        {
        }

        public Task Close(SenderContext context)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateIfNotExist(Channel channel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteIfExist(Channel channel)
        {
            throw new NotImplementedException();
        }

        public Task<Statistic> GetStatistic(Channel channel)
        {
            throw new NotImplementedException();
        }

        public bool IsActive(SenderContext context)
        {
            throw new NotImplementedException();
        }

        public void Open(SenderContext context)
        {
            throw new NotImplementedException();
        }

        public Task<MessageContext> Read(SenderContext sendercontext, MessageContext context, IMessageAdapter adapter)
        {
            throw new NotImplementedException();
        }

        public Task<string> Send(SenderContext context, object message)
        {
            throw new NotImplementedException();
        }
    }
}
