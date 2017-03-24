using System;
using Jal.Router.AzureServiceBus.Interface;

namespace Jal.Router.AzureServiceBus.Fluent.Interface
{
    public interface IToEndPointBuilder
    {
        IReplyToEndPointBuilder To(Func<IBrokeredMessageEndPointSettingValueFinder, string> connectionstringextractor, Func<IBrokeredMessageEndPointSettingValueFinder, string> pathextractor);
    }
}