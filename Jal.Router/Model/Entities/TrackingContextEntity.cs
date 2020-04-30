using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class TrackingContextEntity
    {
        public IList<Tracking> Trackings { get; private set; }

        private TrackingContextEntity()
        {

        }

        public TrackingContextEntity(IList<Tracking> trackings)
        {
            Trackings = trackings;
        }
    }
}
