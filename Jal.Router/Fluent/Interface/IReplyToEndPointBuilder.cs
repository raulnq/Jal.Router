using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IReplyToEndPointBuilder
    {
        void ReplyTo(Func<IEndPointValueSettingFinder, string> connectionstringextractor, Func<IEndPointValueSettingFinder, string> pathextractor);
    }
}