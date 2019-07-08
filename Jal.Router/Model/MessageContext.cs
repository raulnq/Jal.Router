using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Outbound;

namespace Jal.Router.Model
{
    public class MessageContext
    {
        private readonly IBus _bus;
        public Channel Channel { get; private set; }
        public IDictionary<string, string> Headers { get; }
        public DateTime DateTimeUtc { get; }
        public DateTime? ScheduledEnqueueDateTimeUtc { get; }
        public string Version { get; }
        public Route Route { get; private set; }
        public EndPoint EndPoint { get; }
        public Origin Origin { get; }
        public Saga Saga { get; private set; }
        public SagaContext SagaContext { get; private set; }
        public ContentContext ContentContext { get; set; }
        public TrackingContext TrackingContext { get; }
        public IdentityContext IdentityContext { get; }

        public MessageContext()
        {

        }

        public MessageContext(IBus bus, IdentityContext identitycontext, DateTime datetimeutc, List<Track> tracks, Origin origin, string sagaid, string version)
        {
            _bus = bus;
            Headers = new Dictionary<string, string>();
            Version = version;
            Origin = origin;
            SagaContext = new SagaContext(this, sagaid);
            TrackingContext = new TrackingContext(this, tracks);
            IdentityContext = identitycontext;
            DateTimeUtc = datetimeutc;
            ContentContext = new ContentContext();
        }

        public string GetFullName()
        {
            return Saga == null ? Route.Name : Saga.Name + "/" + Route.Name;
        }

        public MessageContext(EndPoint endpoint, Options options, DateTime datetimeutc, Origin origin, ContentContext contentcontext)
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
            TrackingContext = new TrackingContext(this, options.Tracks);
            DateTimeUtc = datetimeutc;
            ContentContext = contentcontext;
        }

        public void UpdateRoute(Route route)
        {
            Route = route;
        }

        public void UpdateChannel(Channel channel)
        {
            Channel = channel;
        }

        public void UpdateSaga(Saga saga)
        {
            Saga = saga;
        }

        public void UpdateSagaContext(SagaContext sagacontext)
        {
            SagaContext = sagacontext;
        }

        public MessageEntity ToEntity()
        {
            var name = string.Empty;

            if (Route != null)
            {
                name = Route.Name;
            }
            else
            {
                name = EndPoint.Name;
            }

            var entity = new MessageEntity(Channel.ToEntity(), Headers, Version, DateTimeUtc, ScheduledEnqueueDateTimeUtc, Route.ToEntity(),
                EndPoint.ToEntity(), Origin.ToEntity(), Saga.ToEntity(), ContentContext.ToEntity(), SagaContext.ToEntity(),
                TrackingContext.ToEntity(), IdentityContext.ToEntity(), name);

            return entity;
        }

        public bool IsSaga()
        {
            return Saga != null;
        }

        public Dictionary<string, string> CopyHeaders()
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

        public Task<TResult> Reply<TContent, TResult>(TContent content, Options options)
        {
            return _bus.Reply<TContent, TResult>(content, options);
        }

        public Task<TResult> Reply<TContent, TResult>(TContent content, Origin origin, Options options)
        {
            return _bus.Reply<TContent, TResult>(content, origin, options);
        }

        public Task<TResult> Reply<TContent, TResult>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            return _bus.Reply<TContent, TResult>(content, endpoint, origin, options);
        }
    }
}