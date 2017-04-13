using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IRetryUsingBuilder
    {
        void Using<TExtractor>(Func<IValueSettingFinder, IRetryPolicy> policycreator) where TExtractor : IValueSettingFinder;
    }
}