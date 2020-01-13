using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Model
{
    public class MessageContext
    {
        private readonly IBus _bus;
        public Channel Channel { get; private set; }
        public IDictionary<string, string> Headers { get; private set; }
        public DateTime DateTimeUtc { get; private set; }
        public string Host { get; private set; }
        public DateTime? ScheduledEnqueueDateTimeUtc { get; private set; }
        public string Version { get; private set; }
        public Route Route { get; private set; }
        public EndPoint EndPoint { get; private set; }
        public Origin Origin { get; private set; }
        public Saga Saga { get; private set; }
        public SagaContext SagaContext { get; private set; }
        public ContentContext ContentContext { get; private set; }
        public TrackingContext TrackingContext { get; private set; }
        public IdentityContext IdentityContext { get; private set; }
        public string Id {
            get
            {
                return IdentityContext?.Id;
            }
        }


        public bool FromSaga()
        {
            return Saga != null && SagaContext!=null && SagaContext.Data != null && SagaContext.Data.Data != null;
        }

        public string Name
        {
            get
            {
                if(EndPoint!=null)
                {
                    return Saga == null ? EndPoint.Name : Saga.Name + "/" + EndPoint.Name;
                }
                else
                {
                    return Saga == null ? Route?.Name : Saga.Name + "/" + Route?.Name;
                }
            }
        }

        public MessageContext(IBus bus): this(bus, Guid.NewGuid().ToString())
        {

        }

        public MessageContext(IBus bus, string id) 
            :this(bus, new IdentityContext(id: id), DateTime.UtcNow, new List<Tracking>(), new Origin(), string.Empty, string.Empty, string.Empty)
        {

        }

        public MessageContext(IBus bus, IdentityContext identitycontext, DateTime datetimeutc, List<Tracking> tracks, Origin origin, string sagaid, string version, string contentid)
        {
            _bus = bus;
            Headers = new Dictionary<string, string>();
            Version = version;
            Origin = origin;
            SagaContext = new SagaContext(this, sagaid);
            TrackingContext = new TrackingContext(this, tracks);
            IdentityContext = identitycontext;
            DateTimeUtc = datetimeutc;
            ContentContext = new ContentContext(this, contentid, !string.IsNullOrEmpty(contentid));
            Host = Environment.MachineName;
        }

        public MessageContext(EndPoint endpoint, Options options, DateTime datetimeutc, Origin origin, Type contenttype, string data, bool isclaimcheck)
        {
            Headers = new Dictionary<string, string>();
            Origin = origin;
            EndPoint = endpoint;
            IdentityContext = options.IdentityContext;
            Headers = options.Headers;
            Version = options.Version;
            ScheduledEnqueueDateTimeUtc = options.ScheduledEnqueueDateTimeUtc;
            SagaContext = options.SagaContext;
            Route = options.Route;
            Saga = options.Saga;
            TrackingContext = options.TrackingContext;
            DateTimeUtc = datetimeutc;
            ContentContext = new ContentContext(this, string.Empty, isclaimcheck, contenttype, data);
            Host = Environment.MachineName;
        }

        public void SetContent(ContentContext contentcontext)
        {
            ContentContext = contentcontext;
        }

        public void SetRoute(Route route)
        {
            Route = route;
        }

        public void SetEndPoint(EndPoint endpoint)
        {
            EndPoint = endpoint;
        }

        public void SetChannel(Channel channel)
        {
            Channel = channel;
        }

        public void SetSaga(Saga saga)
        {
            Saga = saga;
        }

        public void UpdateSagaContext(SagaContext sagacontext)
        {
            SagaContext = sagacontext;
        }

        public MessageEntity ToEntity()
        {
            return new MessageEntity(Host, Channel?.ToEntity(), Headers, Version, DateTimeUtc, ScheduledEnqueueDateTimeUtc, Route?.ToEntity(),
                EndPoint?.ToEntity(), Origin?.ToEntity(), Saga?.ToEntity(), ContentContext?.ToEntity(), SagaContext?.ToEntity(),
                TrackingContext?.ToEntity(), IdentityContext?.ToEntity(), Name);
        }

        public Dictionary<string, string> CreateCopyOfHeaders()
        {
            return Headers.ToDictionary(header => header.Key, header => header.Value);
        }

        public Task FireAndForget<TContent>(TContent content, Options options)
        {
            return _bus.FireAndForget(content, options);
        }

        public Task FireAndForget<TContent>(TContent content, Origin origin, Options options)
        {
            return _bus.FireAndForget(content, Origin, options);
        }

        public Task FireAndForget<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            return _bus.FireAndForget(content, endpoint, origin, options);
        }

        public Task Send<TContent>(TContent content, Options options)
        {
            return _bus.Send(content, options);
        }

        public Task Send<TContent>(TContent content, Origin origin, Options options)
        {
            return _bus.Send(content, origin, options);
        }

        public Task Send<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            return _bus.Send(content, endpoint, origin, options);
        }

        public Task Publish<TContent>(TContent content, Options options)
        {
            return _bus.Publish(content, options);
        }

        public Task Publish<TContent>(TContent content, Origin origin, Options options)
        {
            return _bus.Publish(content, origin, options);
        }

        public Task Publish<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            return _bus.Publish(content, endpoint, origin, options);
        }

        public Task<TResult> Reply<TContent, TResult>(TContent content, Options options) where TResult : class
        {
            return _bus.Reply<TContent, TResult>(content, options);
        }

        public Task<TResult> Reply<TContent, TResult>(TContent content, Origin origin, Options options) where TResult : class
        {
            return _bus.Reply<TContent, TResult>(content, origin, options);
        }

        public Task<TResult> Reply<TContent, TResult>(TContent content, EndPoint endpoint, Origin origin, Options options) where TResult : class
        {
            return _bus.Reply<TContent, TResult>(content, endpoint, origin, options);
        }
    }
}