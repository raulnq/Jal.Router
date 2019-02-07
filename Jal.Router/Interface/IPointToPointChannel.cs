using System;
using Jal.Router.Model.Outbound;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Interface
{
    public interface IPointToPointChannel
    {
        Func<object[]> CreateSenderMethodFactory(SenderMetadata metadata);

        Action<object[]> DestroySenderMethodFactory(SenderMetadata metadata);

        Func<object[], object, string> SendMethodFactory(SenderMetadata metadata);

        Func<object[]> CreateListenerMethodFactory(ListenerMetadata metadata);

        Func<object[], bool> IsActiveMethodFactory(ListenerMetadata metadata);

        Action<object[]> DestroyListenerMethodFactory(ListenerMetadata metadata);

        Action<object[]> ListenerMethodFactory(ListenerMetadata metadata);
    }
}