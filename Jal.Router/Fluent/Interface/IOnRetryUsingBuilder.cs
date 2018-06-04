using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnRetryUsingBuilder
    {
        IOnRouteOptionBuilder Using<TExtractor>(Func<IValueSettingFinder, IRetryPolicy> policycreator) where TExtractor : IValueSettingFinder;

        IOnRouteOptionBuilder Using(IRetryPolicy policy);
    }
}