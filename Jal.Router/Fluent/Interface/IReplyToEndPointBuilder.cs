using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IReplyToEndPointBuilder//TODO delete
    {
        void ReplyTo(Func<IEndPointValueSettingFinder, string> connectionstringextractor, Func<IEndPointValueSettingFinder, string> pathextractor);
    }
}