using System;
using System.Diagnostics;
using Common.Logging;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Logger.Impl
{
    public class BusLogger : IMiddleware
    {
        private readonly ILog _log;

        public BusLogger(ILog log)
        {
            _log = log;
        }

        public object Execute(MessageContext context, Func<MessageContext, MiddlewareContext, object> next, MiddlewareContext middlewarecontext)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            try
            {
                _log.Info($"[Bus.cs, {middlewarecontext.OutboundType}, {context.Identity.Id}] Start Call. id: {context.Identity.Id} sagaid: {context.SagaContext?.Id} endpoint: {context.EndPoint.Name} path: {middlewarecontext.Channel.GetPath()} from: {context.Origin.From} origin: {context.Origin.Key} operationid: {context.Identity.OperationId} parentid: {context.Identity.ParentId}");

                var result = next(context, middlewarecontext);

                _log.Info($"[Bus.cs, {middlewarecontext.OutboundType}, {context.Identity.Id}] Message sent. id: {context.Identity.Id} sagaid: {context.SagaContext?.Id} endpoint: {context.EndPoint.Name} path: {middlewarecontext.Channel.GetPath()} from: {context.Origin.From} origin: {context.Origin.Key}  operationid: {context.Identity.OperationId} parentid: {context.Identity.ParentId}");

                return result;
            }
            catch (Exception exception)
            {
                _log.Error($"[Bus.cs, {middlewarecontext.OutboundType}, {context.Identity.Id}] Exception.  id: {context.Identity.Id} sagaid: {context.SagaContext?.Id} endpoint: {context.EndPoint.Name} path: {middlewarecontext.Channel.GetPath()} from: {context.Origin.From} origin: {context.Origin.Key}  operationid: {context.Identity.OperationId} parentid: {context.Identity.ParentId}", exception);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                _log.Info($"[Bus.cs, {middlewarecontext.OutboundType}, {context.Identity.Id}] End Call. Took {stopwatch.ElapsedMilliseconds} ms. id: {context.Identity.Id} sagaid: {context.SagaContext?.Id} endpoint: {context.EndPoint.Name} path: {middlewarecontext.Channel.GetPath()} from: {context.Origin.From} origin: {context.Origin.Key}  operationid: {context.Identity.OperationId} parentid: {context.Identity.ParentId}");
            }
        }
    }
}