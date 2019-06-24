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
        public object Response { get; set; }
        public IDictionary<string, string> Headers { get; }
        public string Version { get; }
        public Route Route { get; private set; }
        public EndPoint EndPoint { get; }
        public Origin Origin { get; }
        public DateTime DateTimeUtc { get; }
        public SagaContext SagaContext { get; }
        public SagaEntity SagaEntity { get; set; }
        public Saga Saga { get; private set; }
        public DateTime? ScheduledEnqueueDateTimeUtc { get; }
        public Type ContentType { get; set; }
        public string Content { get; set; }
        public string ContentId { get; set; }
        public List<Track> Tracks { get; }
        public IdentityContext IdentityContext { get; }
        public MessageContext(IBus bus, IdentityContext identitycontext, DateTime datetimeutc, List<Track> tracks, Origin origin, string version)
        {
            _bus = bus;
            Headers = new Dictionary<string, string>();
            Version = version;
            Origin = origin;
            SagaContext = new SagaContext();
            Tracks = tracks;
            IdentityContext = identitycontext;
            DateTimeUtc = datetimeutc;
        }

        public string GetFullName()
        {
            return Saga == null ? Route.Name : Saga.Name + "/" + Route.Name;
        }

        public MessageContext(EndPoint endpoint, Options options, DateTime datetimeutc, Origin origin)
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
            Tracks = options.Tracks;
            DateTimeUtc = datetimeutc;

        }

        public void AddTrack(IdentityContext identity, Origin origin, Route route, Saga saga=null, SagaContext sagacontext=null)
        {
            var tracking = new Track()
            {
                Id = identity?.Id,
                Key = origin?.Key,
                From = origin?.From,
                SagaId = sagacontext?.Id,
                Route = route?.Name,
                Saga = saga?.Name
            };

            Tracks.Add(tracking);
        }

        public void UpdateFromRoute(Route route, Channel channel, Saga saga)
        {
            Route = route;

            Channel = channel;

            Saga = saga;
        }

        public void UpdateFromEndpoint(Channel channel)
        {
            Channel = channel;
        }

        public Track[] GetTracksOfTheCurrentSaga()
        {
            if (!string.IsNullOrWhiteSpace(SagaContext.Id))
            {
                return Tracks.Where(x => x.SagaId == SagaContext.Id).ToArray();
            }

            return new Track[] {};
        }

        public Track GetCurrentTrack()
        {
            if (Tracks.Count > 0)
            {
                return Tracks[Tracks.Count - 1];
            }
            return null;
        }

        public Track GetTrackOfTheCaller()
        {
            if (Tracks.Count > 1)
            {
                return Tracks[Tracks.Count - 2];
            }
            return null;
        }

        public Track GetTrackOfTheSagaCaller()
        {
            if (!string.IsNullOrWhiteSpace(SagaContext.Id))
            {
                var index = -1;

                for (var i = 0; i < Tracks.Count; i++)
                {
                    if (Tracks[i].SagaId == SagaContext.Id)
                    {
                        index = i - 1;

                        break;
                    }
                }

                if (index >= 0)
                {
                    return Tracks[index];
                }
            }

            return null;
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