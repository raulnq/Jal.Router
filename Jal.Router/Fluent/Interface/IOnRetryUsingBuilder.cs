using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnRetryUsingBuilder
    {
        IOnErrorBuilder Using<TExtractor>(Func<IValueSettingFinder, IRetryPolicy> policycreator) where TExtractor : IValueSettingFinder;
    }
}