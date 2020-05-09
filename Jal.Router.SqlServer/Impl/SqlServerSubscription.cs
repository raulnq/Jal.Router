using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using System;
using System.Threading.Tasks;

namespace Jal.Router.SqlServer.Impl
{
    public class SqlServerSubscription : AbstractChannel, ISubscriptionToPublishSubscribeChannel
    {
        public SqlServerSubscription(IComponentFactoryFacade factory, ILogger logger) : base(factory, logger)
        {
        }

        public Task Close(ListenerContext context)
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

        public void Listen(ListenerContext context)
        {
            throw new NotImplementedException();
        }

        public void Open(ListenerContext context)
        {
            throw new NotImplementedException();
        }
    }
}
