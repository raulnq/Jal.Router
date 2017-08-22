using System;
using System.Diagnostics;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class SagaRouter<TMessage> : ISagaRouter<TMessage>
    {
        public ISagaRouterProvider Provider { get; set; }

        private readonly ISagaRouterInvoker _invoker;

        public IRouterInterceptor Interceptor { get; set; }

        public IRouterLogger Logger { get; set; }

        private readonly IMessageAdapter<TMessage> _adapter;

        public SagaRouter(IMessageAdapter<TMessage> adapter, ISagaRouterInvoker invoker, ISagaRouterProvider provider)
        {
            _adapter = adapter;
            _invoker = invoker;
            Provider = provider;
            Interceptor = AbstractRouterInterceptor.Instance;
            Logger = AbstractRouterLogger.Instance;
        }

        public void Route<TContent>(TMessage message, string saganame, string routename = "")
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var context = _adapter.Read<TContent>(message);

            Logger.OnEntry(context);

            Interceptor.OnEntry(context);

            try
            {
                var bodytype = typeof(TContent);

                var saga = Provider.Provide(saganame);

                if (saga != null)
                {
                    if (saga.Start!=null && saga.Start.BodyType == bodytype)
                    {
                        _invoker.Start(saga, context, saga.Start);
                    }
                    else
                    {
                        var continueroute = Provider.Provide(saga, bodytype, routename);

                        if (continueroute != null)
                        {
                            _invoker.Continue(saga, context, continueroute);
                        }
                        else
                        {
                            throw new ApplicationException($"No route to handle the Content {nameof(TContent)} and name {routename}");
                        }
                    }
                }
                else
                {
                    throw new ApplicationException($"No saga to handle the Content {nameof(TContent)} and name {saganame}");
                }

                Logger.OnSuccess(context, context.Content);

                Interceptor.OnSuccess(context, context.Content);
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException ?? ex;

                Logger.OnException(context, inner);

                Interceptor.OnException(context, inner);

                throw inner;
            }
            finally
            {
                stopwatch.Stop();

                Logger.OnExit(context, stopwatch.ElapsedMilliseconds);

                Interceptor.OnExit(context);
            }
        }
    }
}