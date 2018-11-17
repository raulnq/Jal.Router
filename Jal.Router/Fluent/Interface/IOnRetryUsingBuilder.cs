using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnRetryUsingBuilder
    {
        IOnRouteOptionBuilder Using<TExtractor>(Func<IValueFinder, IRetryPolicy> policycreator) where TExtractor : IValueFinder;

        IOnRouteOptionBuilder Using(IRetryPolicy policy);
    }
}