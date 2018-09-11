using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnEndPointOptionBuilder : IToEndPointBuilder
    {
        IOnEndPointOptionBuilder UseMiddleware(Action<IOutboundMiddlewareBuilder> action);

        IOnEndPointOptionBuilder AsClaimCheck();
    }
}