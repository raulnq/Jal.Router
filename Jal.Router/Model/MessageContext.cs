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

        private readonly IMessageSerializer _serializer;

        private readonly IEntityStorage _storage;
        public Channel Channel { get; set; }
        public object Response { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public string Version { get; set; }
        public int RetryCount { get; set; }
        public bool LastRetry { get; set; }
        public Route Route { get; set; }
        public EndPoint EndPoint { get; set; }
        public Origin Origin { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public SagaContext SagaContext { get; set; }
        public Saga Saga { get; set; }
        public DateTime? ScheduledEnqueueDateTimeUtc { get; set; }
        public Type ContentType { get; set; }
        public string Content { get; set; }
        public string ContentId { get; set; }
        public List<Track> Tracks { get; set; }
        public IdentityContext IdentityContext { get; set; }
        public MessageContext(IBus bus, IMessageSerializer serializer, IEntityStorage storage)
        {
            _bus = bus;
            _serializer = serializer;
            _storage = storage;
            Headers = new Dictionary<string, string>();
            Version = "1";
            LastRetry = true;
            Origin = new Origin();
            SagaContext = new SagaContext();
            Tracks = new List<Track>();
            IdentityContext = new IdentityContext();
        }

        public MessageContext(EndPoint endpoint, Options options)
        {
            Headers = new Dictionary<string, string>();
            Version = "1";
            LastRetry = true;
            Origin = new Origin();
            EndPoint = endpoint;
            IdentityContext = options.Identity;
            Headers = options.Headers;
            Version = options.Version;
            ScheduledEnqueueDateTimeUtc = options.ScheduledEnqueueDateTimeUtc;
            RetryCount = options.RetryCount;
            SagaContext = options.SagaContext;

            Tracks = options.Tracks;

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

        public async Task Update(object data)
        {
            if (_storage != null && _serializer != null)
            {
                var sagaentity = await _storage.GetSagaEntity(SagaContext.Id).ConfigureAwait(false);

                if (sagaentity != null)
                {
                    sagaentity.Data = _serializer.Serialize(data);

                    sagaentity.Updated = DateTimeUtc;

                    sagaentity.Status = SagaContext.Status;

                    await _storage.UpdateSagaEntity(this, sagaentity).ConfigureAwait(false);
                }
            }
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

        public Task Send<TContent>(TContent content, Options options)
        {
            return _bus.Send(content, options);
        }

        public async Task Send<TContent, TData>(TData data, TContent content, Options options)
        {
            await Update(data).ConfigureAwait(false);

            await _bus.Send(content, options).ConfigureAwait(false);
        }

        public Task Send<TContent>(TContent content, Origin origin, Options options)
        {
            return _bus.Send(content, origin, options);
        }

        public Task Send<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            return _bus.Send(content, endpoint, origin, options);
        }

        public async Task Send<TContent, TData>(TData data, TContent content, Origin origin, Options options)
        {
            await Update(data).ConfigureAwait(false);

            await _bus.Send(content, origin, options).ConfigureAwait(false);
        }

        public Task Publish<TContent>(TContent content, Options options)
        {
            return _bus.Publish(content, options);
        }

        public async Task Publish<TContent, TData>(TData data, TContent content, Options options)
        {
            await Update(data).ConfigureAwait(false);

            await _bus.Publish(content, options).ConfigureAwait(false);
        }

        public Task Publish<TContent>(TContent content, Origin origin, Options options)
        {
            return _bus.Publish(content, origin, options);
        }

        public Task Publish<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            return _bus.Publish(content, endpoint, origin, options);
        }

        public async Task Publish<TContent, TData>(TData data, TContent content, Origin origin, Options options)
        {
            await Update(data).ConfigureAwait(false);

            await _bus.Publish(content, origin, options).ConfigureAwait(false);
        }

        public Task<TResult> Reply<TContent, TResult>(TContent content, Options options)
        {
            return _bus.Reply<TContent, TResult>(content, options);
        }

        public Task<TResult> Reply<TContent, TResult>(TContent content, Origin origin, Options options)
        {
            return _bus.Reply<TContent, TResult>(content, origin, options);
        }

        public async Task<TResult> Reply<TContent, TResult, TData>(TData data, TContent content, Options options)
        {
            await Update(data).ConfigureAwait(false);

            return await _bus.Reply<TContent, TResult>(content, options).ConfigureAwait(false);
        }

        public async Task<TResult> Reply<TContent, TResult, TData>(TData data, TContent content, Origin origin, Options options)
        {
            await Update(data).ConfigureAwait(false);

            return await _bus.Reply<TContent, TResult>(content, origin, options).ConfigureAwait(false);
        }
    }
}