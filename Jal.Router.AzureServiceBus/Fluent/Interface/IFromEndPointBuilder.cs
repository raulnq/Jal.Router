using System;
using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.AzureServiceBus.Model;

namespace Jal.Router.AzureServiceBus.Fluent.Interface
{
    public interface IFromEndPointBuilder : IToEndPointBuilder
    {
        IToEndPointBuilder From(Func<IBrokeredMessageEndPointSettingValueFinder, string> fromextractor);
    }
}