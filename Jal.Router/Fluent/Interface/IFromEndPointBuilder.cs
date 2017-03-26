using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IFromEndPointBuilder : IToEndPointBuilder
    {
        IToEndPointBuilder From(Func<IEndPointValueSettingFinder, string> fromextractor);
    }
}