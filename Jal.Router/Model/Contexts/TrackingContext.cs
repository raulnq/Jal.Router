using System.Collections.Generic;
using System.Linq;

namespace Jal.Router.Model
{
    public class TrackingContext
    {
        public IList<Tracking> Trackings { get; private set; }

        public MessageContext Context { get; private set; }

        public TrackingContext(MessageContext context, IList<Tracking> trackings)
        {
            Context = context;
            Trackings = trackings;
        }

        public TrackingContextEntity ToEntity()
        {
            return new TrackingContextEntity(Trackings);
        }

        public Tracking[] GetTracksOfSaga(string sagaid)
        {
            if (!string.IsNullOrWhiteSpace(sagaid))
            {
                return Trackings.Where(x => x.SagaId == sagaid).ToArray();
            }

            return new Tracking[] { };
        }

        public Tracking[] GetTracksOfTheCurrentSaga()
        {
            if (!string.IsNullOrWhiteSpace(Context.SagaContext.Data.Id))
            {
                return Trackings.Where(x => x.SagaId == Context.SagaContext.Data.Id).ToArray();
            }

            return new Tracking[] { };
        }

        public Tracking GetCurrentTrack()
        {
            if (Trackings.Count > 0)
            {
                return Trackings[Trackings.Count - 1];
            }
            return null;
        }

        public Tracking GetTrackOfTheCaller()
        {
            if (Trackings.Count > 1)
            {
                return Trackings[Trackings.Count - 2];
            }
            return null;
        }

        public Tracking GetTrackOfTheSagaCaller()
        {
            if (!string.IsNullOrWhiteSpace(Context.SagaContext.Data.Id))
            {
                var index = -1;

                for (var i = 0; i < Trackings.Count; i++)
                {
                    if (Trackings[i].SagaId == Context.SagaContext.Data.Id)
                    {
                        index = i - 1;

                        break;
                    }
                }

                if (index >= 0)
                {
                    return Trackings[index];
                }
            }

            return null;
        }

        public void AddEntry()
        {
            var tracking = new Tracking(Context.Id, Context.SagaContext?.Data?.Id, Context.Origin?.From,
                 Context.Origin?.Key, Context.Route?.Name, Context.Saga?.Name);

            Trackings.Add(tracking);
        }
    }
}