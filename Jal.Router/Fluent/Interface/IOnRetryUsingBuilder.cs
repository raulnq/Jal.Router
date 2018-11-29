using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnRetryUsingBuilder
    {
        IOnRouteOptionBuilder With<TValueFinder>(Func<IValueFinder, IRetryPolicy> policycreator) where TValueFinder : IValueFinder;

        IOnRouteOptionBuilder With(IRetryPolicy policy);
    }
}