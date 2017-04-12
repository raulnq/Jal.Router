using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IFromEndPointBuilder : IToEndPointBuilder
    {
        IToEndPointBuilder From(Func<IValueSettingFinder, string> fromextractor);
    }
}