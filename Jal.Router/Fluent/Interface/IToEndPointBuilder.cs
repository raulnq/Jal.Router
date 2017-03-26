using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IToEndPointBuilder
    {
        IReplyToEndPointBuilder To(Func<IEndPointValueSettingFinder, string> connectionstringextractor, Func<IEndPointValueSettingFinder, string> pathextractor);
    }
}