using System;
using Jal.Router.Interface;
using Jal.Router.Model.Outbound;
using Jal.Router.Model.Inbound;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class NullPublishSubscribeChannel : IPublishSubscribeChannel
    {
        public void Open(ListenerMetadata metadata)
        {

        }

        public bool IsActive()
        {
            return false;
        }

        public Task Close()
        {
            return Task.CompletedTask;
        }

        public void Listen()
        {

        }

        public void Open(SenderMetadata metadata)
        {

        }

        public Task<string> Send(object message)
        {
            return Task.FromResult(string.Empty);
        }
    }
}