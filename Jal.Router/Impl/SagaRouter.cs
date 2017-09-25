using System;
using System.Diagnostics;
using Jal.Router.Interface;
using Jal.Router.Model;

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

            var context = _adapter.ReadContext(message);

            Logger.OnEntry(context);

            Interceptor.OnEntry(context);

            try
            {
                var contenttype = typeof(TContent);

                var saga = Provider.Provide(saganame);

                if (saga != null)
                {
                    if (saga.Start!=null && saga.Start.BodyType == contenttype)
                    {
                        var content = _adapter.ReadContent<TContent>(message);

                        _invoker.Start(saga, new InboundMessageContext<TContent>(context, content), saga.Start);

                        Logger.OnSuccess(context, content);

                        Interceptor.OnSuccess(context, content);
                    }
                    else
                    {
                        var continueroute = Provider.Provide(saga, contenttype, routename);

                        if (continueroute != null)
                        {
                            var content = _adapter.ReadContent<TContent>(message);

                            _invoker.Continue(saga, new InboundMessageContext<TContent>(context, content), continueroute);

                            Logger.OnSuccess(context, content);

                            Interceptor.OnSuccess(context, content);
                        }
                        else
                        {
                            throw new ApplicationException($"No route to handle the message {typeof(TMessage).FullName} with content {contenttype.FullName} and route name {routename}");
                        }
                    }
                }
                else
                {
                    throw new ApplicationException($"No saga to handle the message {typeof(TMessage).FullName} with content {contenttype.FullName} and saga name {saganame}");
                }

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