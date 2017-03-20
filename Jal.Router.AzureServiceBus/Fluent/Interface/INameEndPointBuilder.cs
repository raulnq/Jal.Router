using System;
using Jal.Router.AzureServiceBus.Interface;

namespace Jal.Router.AzureServiceBus.Fluent.Interface
{
    public interface INameEndPointBuilder
    {
        IFromEndPointBuilder ForMessage<TMessage>();
    }
}