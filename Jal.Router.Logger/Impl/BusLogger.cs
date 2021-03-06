using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Common.Logging;
using Jal.ChainOfResponsability;
using Jal.Router.Model;

namespace Jal.Router.Logger
{
    public class BusLogger : IAsyncMiddleware<MessageContext>
    {
        private readonly ILog _log;

        public BusLogger(ILog log)
        {
            _log = log;
        }

        public async Task ExecuteAsync(AsyncContext<MessageContext> context, Func<AsyncContext<MessageContext>, Task> next)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            try
            {
                _log.Debug($"[Bus.cs, {context.Data.Channel.ToString()}, {context.Data.Id}] Start Call. id: {context.Data.Id} sagaid: {context.Data.SagaContext?.Data?.Id} endpoint: {context.Data.EndPoint.Name} path: {context.Data.Channel.FullPath} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key} operationid: {context.Data.TracingContext.OperationId} parentid: {context.Data.TracingContext.ParentId}");

                await next(context).ConfigureAwait(false);

                _log.Info($"[Bus.cs, {context.Data.Channel.ToString()}, {context.Data.Id}] Message sent. id: {context.Data.Id} sagaid: {context.Data.SagaContext?.Data?.Id} endpoint: {context.Data.EndPoint.Name} path: {context.Data.Channel.FullPath} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key}  operationid: {context.Data.TracingContext.OperationId} parentid: {context.Data.TracingContext.ParentId}");
            }
            catch (Exception exception)
            {
                _log.Error($"[Bus.cs, {context.Data.Channel.ToString()}, {context.Data.Id}] Exception.  id: {context.Data.Id} sagaid: {context.Data.SagaContext?.Data?.Id} endpoint: {context.Data.EndPoint.Name} path: {context.Data.Channel.FullPath} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key}  operationid: {context.Data.TracingContext.OperationId} parentid: {context.Data.TracingContext.ParentId}", exception);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                _log.Debug($"[Bus.cs, {context.Data.Channel.ToString()}, {context.Data.Id}] End Call. Took {stopwatch.ElapsedMilliseconds} ms. id: {context.Data.Id} sagaid: {context.Data.SagaContext?.Data?.Id} endpoint: {context.Data.EndPoint.Name} path: {context.Data.Channel.FullPath} from: {context.Data.Origin.From} origin: {context.Data.Origin.Key}  operationid: {context.Data.TracingContext.OperationId} parentid: {context.Data.TracingContext.ParentId}");
            }
        }
    }
}