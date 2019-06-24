using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnEndPointOptionBuilder : IToEndPointBuilder, IToReplyEndPointBuilder
    {
        IOnEndPointOptionBuilder UseMiddleware(Action<IOutboundMiddlewareBuilder> action);

        IOnEndPointOptionBuilder AsClaimCheck();


    }
}