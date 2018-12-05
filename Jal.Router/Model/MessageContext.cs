using System;
using System.Collections.Generic;
using System.Linq;
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
        public Type ResultType { get; set; }
        public string Content { get; set; }
        public string ContentId { get; set; }
        public List<Track> Tracks { get; set; }
        public Identity Identity { get; set; }
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
            Identity = new Identity();
        }

        public MessageContext(EndPoint endpoint, Options options)
        {
            Headers = new Dictionary<string, string>();
            Version = "1";
            LastRetry = true;
            Origin = new Origin();
            EndPoint = endpoint;
            Identity = options.Identity;
            Headers = options.Headers;
            Version = options.Version;
            ScheduledEnqueueDateTimeUtc = options.ScheduledEnqueueDateTimeUtc;
            RetryCount = options.RetryCount;
            SagaContext = options.SagaContext;

            Tracks = options.Tracks;

        }

        public void AddTrack(Identity identity, Origin origin, Route route, Saga saga=null, SagaContext sagacontext=null)
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

        public Track[] GetTracksOfTheSaga()
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

        public void Update(object data)
        {
            if (_storage != null && _serializer != null)
            {
                var sagaentity = _storage.GetSagaEntity(SagaContext.Id);

                if (sagaentity != null)
                {
                    sagaentity.Data = _serializer.Serialize(data);

                    sagaentity.Updated = DateTimeUtc;

                    sagaentity.Status = SagaContext.Status;

                    _storage.UpdateSagaEntity(this, sagaentity);
                }
            }
        }

        public Dictionary<string, string> CopyHeaders()
        {
            return Headers.ToDictionary(header => header.Key, header => header.Value);
        }

        public void FireAndForget<TContent>(TContent content, Options options)
        {
            _bus.FireAndForget(content, options);
        }

        public void FireAndForget<TContent>(TContent content, Origin origin, Options options)
        {
            _bus.FireAndForget(content, Origin, options);
        }

        public void Send<TContent>(TContent content, Options options)
        {
            _bus.Send(content, options);
        }

        public void Send<TContent, TData>(TData data, TContent content, Options options)
        {
            Update(data);

            _bus.Send(content, options);
        }

        public void Send<TContent>(TContent content, Origin origin, Options options)
        {
            _bus.Send(content, origin, options);
        }

        public void Send<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            _bus.Send(content, endpoint, origin, options);
        }

        public void Send<TContent, TData>(TData data, TContent content, Origin origin, Options options)
        {
            Update(data);

            _bus.Send(content, origin, options);
        }

        public void Publish<TContent>(TContent content, Options options)
        {
            _bus.Publish(content, options);
        }

        public void Publish<TContent, TData>(TData data, TContent content, Options options)
        {
            Update(data);

            _bus.Publish(content, options);
        }

        public void Publish<TContent>(TContent content, Origin origin, Options options)
        {
            _bus.Publish(content, origin, options);
        }

        public void Publish<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            _bus.Publish(content, endpoint, origin, options);
        }

        public void Publish<TContent, TData>(TData data, TContent content, Origin origin, Options options)
        {
            Update(data);

            _bus.Publish(content, origin, options);
        }

        public TResult Reply<TContent, TResult>(TContent content, Options options)
        {
            return _bus.Reply<TContent, TResult>(content, options);
        }

        public TResult Reply<TContent, TResult>(TContent content, Origin origin, Options options)
        {
            return _bus.Reply<TContent, TResult>(content, origin, options);
        }

        public TResult Reply<TContent, TResult, TData>(TData data, TContent content, Options options)
        {
            Update(data);

            return _bus.Reply<TContent, TResult>(content, options);
        }

        public TResult Reply<TContent, TResult, TData>(TData data, TContent content, Origin origin, Options options)
        {
            Update(data);

            return _bus.Reply<TContent, TResult>(content, origin, options);
        }
    }
}