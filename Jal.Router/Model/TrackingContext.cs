using System.Collections.Generic;
using System.Linq;

namespace Jal.Router.Model
{
    public class TrackingContext
    {
        public List<Track> Tracks { get; }

        public MessageContext Context { get; }

        public TrackingContext(MessageContext context)
        {
            Context = context;
            Tracks = new List<Track>();
        }

        public TrackingContext(MessageContext context, List<Track> tracks):this(context)
        {
            Context = context;
        }

        public TrackingContextEntity ToEntity()
        {
            return new TrackingContextEntity(Tracks);
        }
        public Track[] GetTracksOfTheCurrentSaga()
        {
            if (!string.IsNullOrWhiteSpace(Context.SagaContext.SagaData.Id))
            {
                return Tracks.Where(x => x.SagaId == Context.SagaContext.SagaData.Id).ToArray();
            }

            return new Track[] { };
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
            if (!string.IsNullOrWhiteSpace(Context.SagaContext.SagaData.Id))
            {
                var index = -1;

                for (var i = 0; i < Tracks.Count; i++)
                {
                    if (Tracks[i].SagaId == Context.SagaContext.SagaData.Id)
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

        public void Add()
        {
            var tracking = new Track()
            {
                Id = Context.IdentityContext?.Id,
                Key = Context.Origin?.Key,
                From = Context.Origin?.From,
                SagaId = Context.SagaContext?.SagaData?.Id,
                RouteName = Context.Route?.Name,
                SagaName = Context.Saga?.Name
            };

            Tracks.Add(tracking);
        }
    }
}