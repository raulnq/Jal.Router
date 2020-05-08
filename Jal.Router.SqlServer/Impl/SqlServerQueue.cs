using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using System;
using System.Threading.Tasks;

namespace Jal.Router.SqlServer.Impl
{
    public class SqlServerQueue : AbstractChannel, IPointToPointChannel
    {
        public SqlServerQueue(IComponentFactoryFacade factory, ILogger logger) : base(factory, logger)
        {
        }

        public Task Close(ListenerContext context)
        {
            throw new NotImplementedException();
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

        public bool IsActive(ListenerContext context)
        {
            throw new NotImplementedException();
        }

        public bool IsActive(SenderContext context)
        {
            throw new NotImplementedException();
        }

        public void Listen(ListenerContext context)
        {
            throw new NotImplementedException();
        }

        public void Open(ListenerContext context)
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
