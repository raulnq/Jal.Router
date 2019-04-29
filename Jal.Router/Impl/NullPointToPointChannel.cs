using System;
using Jal.Router.Interface;
using Jal.Router.Model.Outbound;
using Jal.Router.Model.Inbound;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class NullPointToPointChannel : IPointToPointChannel
    {
        public Func<object[]> CreateSenderMethodFactory(SenderMetadata metadata)
        {
            return () => new object[] { };
        }

        public Func<object[], Task> DestroySenderMethodFactory(SenderMetadata metadata)
        {
            return o => { return Task.CompletedTask; };
        }

        public Func<object[], object, string> SendMethodFactory(SenderMetadata metadata)
        {
            return (o, m) => string.Empty;
        }

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

        public string Send(object message)
        {
            return string.Empty;
        }
    }
}