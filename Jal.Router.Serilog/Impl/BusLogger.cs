using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Serilog;
using Jal.ChainOfResponsability;
using Jal.Router.Model;

namespace Jal.Router.Serilog
{
    public class BusLogger : IAsyncMiddleware<MessageContext>
    {
        public async Task ExecuteAsync(AsyncContext<MessageContext> context, Func<AsyncContext<MessageContext>, Task> next)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var log = Log.Logger;

            if (!string.IsNullOrWhiteSpace(context.Data.TracingContext.ParentId))
            {
                log = log.ForContext("ParentId", context.Data.TracingContext.ParentId);
            }

            if (!string.IsNullOrWhiteSpace(context.Data.TracingContext.OperationId))
            {
                log = log.ForContext("OperationId", context.Data.TracingContext.OperationId);
            }

            if (!string.IsNullOrWhiteSpace(context.Data.SagaContext?.Data?.Id))
            {
                log = log.ForContext("SagaId", context.Data.SagaContext?.Data?.Id);
            }

            if (!string.IsNullOrWhiteSpace(context.Data.Origin.From))
            {
                log = log.ForContext("From", context.Data.Origin.From);
            }

            if (!string.IsNullOrWhiteSpace(context.Data.Origin.Key))
            {
                log = log.ForContext("Origin", context.Data.Origin.Key);
            }

            try
            {
                log.Debug("[Bus, {Channel}, {Id}] Start Call. endpoint: {Endpoint} path: {Path}", context.Data.Channel.ToString(), context.Data.Id, context.Data.EndPoint.Name, context.Data.Channel.FullPath);

                await next(context).ConfigureAwait(false);

                log.ForContext("Type","message-producer").Information("[Bus, {Channel}, {Id}] Message sent. endpoint: {Endpoint} path: {Path}", context.Data.Channel.ToString(), context.Data.Id, context.Data.EndPoint.Name, context.Data.Channel.FullPath);
            }
            catch (Exception exception)
            {
                log.Error(exception, "[Bus, {Channel}, {Id}] Exception. endpoint: {Endpoint} path: {Path}", context.Data.Channel.ToString(), context.Data.Id, context.Data.EndPoint.Name, context.Data.Channel.FullPath);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                log.Debug("[Bus, {Channel}, {Id}] End Call. Took {Duration} ms. endpoint: {Endpoint} path: {Path}", context.Data.Channel.ToString(), context.Data.Id, stopwatch.ElapsedMilliseconds, context.Data.EndPoint.Name, context.Data.Channel.FullPath);
            }
        }
    }
}