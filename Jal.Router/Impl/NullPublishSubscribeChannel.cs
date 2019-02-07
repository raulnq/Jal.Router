using System;
using Jal.Router.Interface;
using Jal.Router.Model.Outbound;
using Jal.Router.Model.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class NullPublishSubscribeChannel : IPublishSubscribeChannel
    {
        public Func<object[]> CreateSenderMethodFactory(SenderMetadata metadata)
        {
            return () => new object[] { };
        }

        public Action<object[]> DestroySenderMethodFactory(SenderMetadata metadata)
        {
            return o => { };
        }

        public Func<object[], object, string> SendMethodFactory(SenderMetadata metadata)
        {
            return (o, m) => string.Empty;
        }

        public Func<object[]> CreateListenerMethodFactory(ListenerMetadata metadata)
        {
            return () => new object[] { };
        }

        public Action<object[]> DestroyListenerMethodFactory(ListenerMetadata metadata)
        {
            return o => { };
        }

        public Action<object[]> ListenerMethodFactory(ListenerMetadata metadata)
        {
            return o => { };
        }

        public Func<object[], bool> IsActiveMethodFactory(ListenerMetadata metadata)
        {
            return o => { return true; };
        }
    }
}