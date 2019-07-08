using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Common.Logging;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Model;

namespace Jal.Router.Logger.Impl
{
    public class BusLogger : IMiddlewareAsync<MessageContext>
    {
        private readonly ILog _log;

        public BusLogger(ILog log)
        {
            _log = log;
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            try
            {
                _log.Info($"[Bus.cs, {context.Data.Channel.ToString()}, {context.Data.IdentityContext.Id}] Start Call. id: {context.Data.IdentityContext.Id} sagaid: {context.Data.SagaContext?.SagaData?.Id} endpoint: {context.Data.EndPoint.Name} path: {context.Data.Channel.GetPath()} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key} operationid: {context.Data.IdentityContext.OperationId} parentid: {context.Data.IdentityContext.ParentId}");

                await next(context);

                _log.Info($"[Bus.cs, {context.Data.Channel.ToString()}, {context.Data.IdentityContext.Id}] Message sent. id: {context.Data.IdentityContext.Id} sagaid: {context.Data.SagaContext?.SagaData?.Id} endpoint: {context.Data.EndPoint.Name} path: {context.Data.Channel.GetPath()} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key}  operationid: {context.Data.IdentityContext.OperationId} parentid: {context.Data.IdentityContext.ParentId}");
            }
            catch (Exception exception)
            {
                _log.Error($"[Bus.cs, {context.Data.Channel.ToString()}, {context.Data.IdentityContext.Id}] Exception.  id: {context.Data.IdentityContext.Id} sagaid: {context.Data.SagaContext?.SagaData?.Id} endpoint: {context.Data.EndPoint.Name} path: {context.Data.Channel.GetPath()} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key}  operationid: {context.Data.IdentityContext.OperationId} parentid: {context.Data.IdentityContext.ParentId}", exception);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                _log.Info($"[Bus.cs, {context.Data.Channel.ToString()}, {context.Data.IdentityContext.Id}] End Call. Took {stopwatch.ElapsedMilliseconds} ms. id: {context.Data.IdentityContext.Id} sagaid: {context.Data.SagaContext?.SagaData?.Id} endpoint: {context.Data.EndPoint.Name} path: {context.Data.Channel.GetPath()} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key}  operationid: {context.Data.IdentityContext.OperationId} parentid: {context.Data.IdentityContext.ParentId}");
            }
        }
    }
}