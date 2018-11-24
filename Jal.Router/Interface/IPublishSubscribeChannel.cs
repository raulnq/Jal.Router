using System;
using Jal.Router.Model.Inbound;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Interface
{
    public interface IPublishSubscribeChannel
    {
        Func<object[]> CreateSenderMethodFactory(SenderMetadata metadata);

        Action<object[]> DestroySenderMethodFactory(SenderMetadata metadata);

        Func<object[], object, string> SendMethodFactory(SenderMetadata metadata);

        Func<object[]> CreateListenerMethodFactory(ListenerMetadata metadata);

        Action<object[]> DestroyListenerMethodFactory(ListenerMetadata metadata);

        Action<object[]> ListenerMethodFactory(ListenerMetadata metadata);
    }
}