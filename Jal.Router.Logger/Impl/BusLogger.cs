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
                _log.Info($"[Bus.cs, {parameter.OutboundType}, {context.Id}] Start Call. id: {context.Id} sagaid: {context.SagaContext?.Id} endpoint: {context.EndPoint.Name} connectionstring: {parameter.Channel.ToConnectionString} path: {parameter.Channel.ToPath} from: {context.Origin.From} origin: {context.Origin.Key}");

                next();

                _log.Info($"[Bus.cs, {parameter.OutboundType}, {context.Id}] Message sent. id: {context.Id} sagaid: {context.SagaContext?.Id} endpoint: {context.EndPoint.Name} connectionstring: {parameter.Channel.ToConnectionString} path: {parameter.Channel.ToPath} from: {context.Origin.From} origin: {context.Origin.Key}");
            }
            catch (Exception exception)
            {
                _log.Error($"[Bus.cs, {parameter.OutboundType}, {context.Id}] Exception.", exception);

                throw;
            }
            finally
            {
                stopwatch.Stop();

                _log.Info($"[Bus.cs, {parameter.OutboundType}, {context.Id}] End Call. Took {stopwatch.ElapsedMilliseconds} ms. id: {context.Id} sagaid: {context.SagaContext?.Id} endpoint: {context.EndPoint.Name} connectionstring: {parameter.Channel.ToConnectionString} path: {parameter.Channel.ToPath} from: {context.Origin.From} origin: {context.Origin.Key}");
            }
        }
    }
}