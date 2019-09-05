using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Serilog;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Model;

namespace Jal.Router.Serilog.Impl
{
    public class BusLogger : IMiddlewareAsync<MessageContext>
    {
        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var log = Log.Logger;

            if (!string.IsNullOrWhiteSpace(context.Data.IdentityContext.ParentId))
            {
                log = log.ForContext("parentid", context.Data.IdentityContext.ParentId);
            }

            if (!string.IsNullOrWhiteSpace(context.Data.IdentityContext.OperationId))
            {
                log = log.ForContext("operationid", context.Data.IdentityContext.OperationId);
            }

            if (!string.IsNullOrWhiteSpace(context.Data.SagaContext?.SagaData?.Id))
            {
                log = log.ForContext("sagaid", context.Data.SagaContext?.SagaData?.Id);
            }

            if (!string.IsNullOrWhiteSpace(context.Data.Origin.From))
            {
                log = log.ForContext("from", context.Data.Origin.From);
            }

            if (!string.IsNullOrWhiteSpace(context.Data.Origin.Key))
            {
                log = log.ForContext("origin", context.Data.Origin.Key);
            }

            try
            {
                log.Information("[Bus.cs, {channel}, {id}] Start Call. endpoint: {endpoint} path: {path}", context.Data.Channel.ToString(), context.Data.Id, context.Data.EndPoint.Name, context.Data.Channel.FullPath);

                await next(context);

                log.Information("[Bus.cs, {channel}, {id}] Message sent. endpoint: {endpoint} path: {path}", context.Data.Channel.ToString(), context.Data.Id, context.Data.EndPoint.Name, context.Data.Channel.FullPath);
            }
            catch (Exception exception)
            {
                log.Error(exception, "[Bus.cs, {channel}, {id}] Exception. endpoint: {endpoint} path: {path}", context.Data.Channel.ToString(), context.Data.Id, context.Data.EndPoint.Name, context.Data.Channel.FullPath);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                log.Information("[Bus.cs, {channel}, {id}] End Call. Took {duration} ms. endpoint: {endpoint} path: {path}", context.Data.Channel.ToString(), context.Data.Id, stopwatch.ElapsedMilliseconds, context.Data.EndPoint.Name, context.Data.Channel.FullPath);
            }
        }
    }
}