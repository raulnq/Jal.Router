using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IToEndPointBuilder
    {
        void To(Func<IValueSettingFinder, string> connectionstringextractor, Func<IValueSettingFinder, string> pathextractor);
    }
}