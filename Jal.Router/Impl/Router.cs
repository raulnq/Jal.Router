using System;
using System.Diagnostics;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class Router<TMessage> : IRouter<TMessage>
    {
        public IRouteProvider Provider { get; set; }

        private readonly INoTypedRouter _notypedrouter;

        public IRouterInterceptor Interceptor { get; set; }

        public IRouterLogger Logger { get; set; }

        private readonly IMessageAdapter<TMessage> _adapter;

        public Router(IMessageAdapter<TMessage> adapter, IRouteProvider provider, INoTypedRouter notypedrouter)
        {
            _adapter = adapter;

            Provider = provider;

            _notypedrouter = notypedrouter;

            Interceptor = AbstractRouterInterceptor.Instance;

            Logger = AbstractRouterLogger.Instance;
        }

        public void Route<TContent>(TMessage message, string routename = "")
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var context = _adapter.Read<TContent>(message);

            Logger.OnEntry(context);

            Interceptor.OnEntry(context);

            try
            {
                var bodytype = typeof(TContent);

                var routes = Provider.Provide(bodytype, routename);

                _notypedrouter.Route(context, routes);

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

    public interface ISagaRouter<in TMessage>
    {
        void Route<TContent>(TMessage message, string saganame = "", string routename = "");
    }

    public interface IStorage
    {
        void Insert(SagaRecord sagarecord);

        void Update(SagaRecord sagarecord);

        SagaRecord Select(InboundMessageContext context);


    }

    
    public class SagaRecord
    {
        public object Data { get; set; }

        public string Name { get; set; }

        public DateTime UtcCreationDateTime { get; set; }
    }

    public class MessageRecord
    {
        public InboundMessageContext Context { get; set; }
    }


    public class SagaRouter<TMessage> : ISagaRouter<TMessage>
    {
        public ISagaProvider Provider { get; set; }

        private readonly INoTypedRouter _notypedrouter;

        public IRouterInterceptor Interceptor { get; set; }

        public IRouterLogger Logger { get; set; }

        private readonly IMessageAdapter<TMessage> _adapter;

        private readonly IStorage _storage;

        public void Route<TContent>(TMessage message, string saganame = "", string routename = "")
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var context = _adapter.Read<TContent>(message);

            Logger.OnEntry(context);

            Interceptor.OnEntry(context);

            try
            {
                var bodytype = typeof(TContent);

                var sagas = Provider.Provide(bodytype, saganame);

                foreach (var saga in sagas)
                {
                    if (Provider.IsTheFirst(saga, bodytype))
                    {
                        var route = Provider.GetFirst(saga, bodytype);

                        Start(context, route);
                    }
                    else
                    {
                        var routes = Provider.GetContinue(saga, bodytype, routename);

                        Continue(context, routes);
                    }
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

        public void Continue<TContent>(InboundMessageContext<TContent> context, Route route)
        {
            
            var record = new SagaRecord() {};
            //create data
            //_notypedrouter.Route(content, context, new Route[] { route }, null);
            //save data
        }

        public void Start<TContent>(InboundMessageContext<TContent> context, Route route)
        {
            var record = new SagaRecord() { };
            _storage.Insert(record);
            //get data
            //_notypedrouter.Route(context.Content, context, saga.Routes.ToArray());
            //save data
        }
    }
}
