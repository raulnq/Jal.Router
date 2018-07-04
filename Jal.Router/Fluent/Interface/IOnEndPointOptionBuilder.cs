using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnEndPointOptionBuilder : IToEndPointBuilder
    {
        IOnEndPointOptionBuilder UsingMiddleware(Action<IOutboundMiddlewareBuilder> action);

        IOnEndPointOptionBuilder AsClaimCheck();
    }
}