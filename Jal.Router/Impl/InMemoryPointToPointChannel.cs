using Jal.Router.Interface;
using Jal.Router.Model;
using System;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class InMemoryPointToPointChannel : AbstractChannel, IPointToPointChannel
    {
        public InMemoryPointToPointChannel(IComponentFactoryGateway factory, ILogger logger) : base(factory, logger)
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

        public Task<string> Send(SenderContext context, object message)
        {
            throw new NotImplementedException();
        }
    }
}