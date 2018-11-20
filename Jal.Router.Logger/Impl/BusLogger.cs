using System;
using System.Diagnostics;
using Common.Logging;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Logger.Impl
{
    public class BusLogger : IMiddleware<MessageContext>
    {
        private readonly ILog _log;

        public BusLogger(ILog log)
        {
            _log = log;
        }

        public void Execute(Context<MessageContext> context, Action<Context<MessageContext>> next)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            try
            {
                _log.Info($"[Bus.cs, {context.Data.Channel.ToString()}, {context.Data.Identity.Id}] Start Call. id: {context.Data.Identity.Id} sagaid: {context.Data.SagaContext?.Id} endpoint: {context.Data.EndPoint.Name} path: {context.Data.Channel.GetPath()} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key} operationid: {context.Data.Identity.OperationId} parentid: {context.Data.Identity.ParentId}");

                next(context);

                _log.Info($"[Bus.cs, {context.Data.Channel.ToString()}, {context.Data.Identity.Id}] Message sent. id: {context.Data.Identity.Id} sagaid: {context.Data.SagaContext?.Id} endpoint: {context.Data.EndPoint.Name} path: {context.Data.Channel.GetPath()} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key}  operationid: {context.Data.Identity.OperationId} parentid: {context.Data.Identity.ParentId}");
            }
            catch (Exception exception)
            {
                _log.Error($"[Bus.cs, {context.Data.Channel.ToString()}, {context.Data.Identity.Id}] Exception.  id: {context.Data.Identity.Id} sagaid: {context.Data.SagaContext?.Id} endpoint: {context.Data.EndPoint.Name} path: {context.Data.Channel.GetPath()} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key}  operationid: {context.Data.Identity.OperationId} parentid: {context.Data.Identity.ParentId}", exception);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                _log.Info($"[Bus.cs, {context.Data.Channel.ToString()}, {context.Data.Identity.Id}] End Call. Took {stopwatch.ElapsedMilliseconds} ms. id: {context.Data.Identity.Id} sagaid: {context.Data.SagaContext?.Id} endpoint: {context.Data.EndPoint.Name} path: {context.Data.Channel.GetPath()} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key}  operationid: {context.Data.Identity.OperationId} parentid: {context.Data.Identity.ParentId}");
            }
        }
    }
}