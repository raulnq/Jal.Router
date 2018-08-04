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

        public void Execute(MessageContext context, Action next, Action curent, MiddlewareParameter parameter)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            try
            {
                _log.Info($"[Bus.cs, {parameter.OutboundType}, {context.Identity.Id}] Start Call. id: {context.Identity.Id} sagaid: {context.SagaContext?.Id} endpoint: {context.EndPoint.Name} path: {parameter.Channel.GetPath()} from: {context.Origin.From} origin: {context.Origin.Key} operationid: {context.Identity.OperationId} parentid: {context.Identity.ParentId}");

                next();

                _log.Info($"[Bus.cs, {parameter.OutboundType}, {context.Identity.Id}] Message sent. id: {context.Identity.Id} sagaid: {context.SagaContext?.Id} endpoint: {context.EndPoint.Name} path: {parameter.Channel.GetPath()} from: {context.Origin.From} origin: {context.Origin.Key}  operationid: {context.Identity.OperationId} parentid: {context.Identity.ParentId}");
            }
            catch (Exception exception)
            {
                _log.Error($"[Bus.cs, {parameter.OutboundType}, {context.Identity.Id}] Exception.  id: {context.Identity.Id} sagaid: {context.SagaContext?.Id} endpoint: {context.EndPoint.Name} path: {parameter.Channel.GetPath()} from: {context.Origin.From} origin: {context.Origin.Key}  operationid: {context.Identity.OperationId} parentid: {context.Identity.ParentId}", exception);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                _log.Info($"[Bus.cs, {parameter.OutboundType}, {context.Identity.Id}] End Call. Took {stopwatch.ElapsedMilliseconds} ms. id: {context.Identity.Id} sagaid: {context.SagaContext?.Id} endpoint: {context.EndPoint.Name} path: {parameter.Channel.GetPath()} from: {context.Origin.From} origin: {context.Origin.Key}  operationid: {context.Identity.OperationId} parentid: {context.Identity.ParentId}");
            }
        }
    }
}